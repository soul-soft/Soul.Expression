﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Soul.Expressions.Tokens;

namespace Soul.Expressions
{
	internal static class SyntaxUtility
	{
		/// <summary>
		/// 是否为常量
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool TryConstantToken(string expr, out Type token)
		{
			if (IsIntgerToken(expr))
			{
				token = typeof(int);
				return true;
			}
			if (IsBoolToken(expr))
			{
				token = typeof(bool);
				return true;
			}
			if (IsDoubleToken(expr))
			{
				token = typeof(double);
				return true;
			}
			if (IsStringToken(expr))
			{
				token = typeof(string);
				return true;
			}
			if (IsCharToken(expr))
			{
				token = typeof(char);
				return true;
			}
			token = null;
			return false;
		}
		/// <summary>
		/// 是否为字符串常量
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsStringToken(string expr)
		{
			if (expr.Length < 2)
			{
				return false;
			}
			var text = Regex.Replace(expr, @"\\.{1}", "#");
			if (!text.StartsWith("\"") || !text.EndsWith("\""))
			{
				return false;
			}
			if (text.Substring(1, text.Length - 2).Contains('"'))
			{
				return false;
			}
			return true;
		}
		/// <summary>
		/// 是否为字符串
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsCharToken(string expr)
		{
			if (expr.Length < 3)
			{
				return false;
			}
			var text = Regex.Replace(expr, @"\\.{1}", "#");
			if (!text.StartsWith("'") || !text.EndsWith("'"))
			{
				return false;
			}
			if (text.Trim('\'').Length != 1)
			{
				return false;
			}
			if (text.Substring(1, text.Length - 2).Contains('\''))
			{
				return false;
			}
			return true;
		}
		/// <summary>
		/// 是否为整数
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsIntgerToken(string expr)
		{
			return Regex.IsMatch(expr, @"^\d+$");
		}
		/// <summary>
		/// 是否为浮点数
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsDoubleToken(string expr)
		{
			return Regex.IsMatch(expr, @"^\d+\.\d+$");
		}
		/// <summary>
		/// 是否为布尔值
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsBoolToken(string expr)
		{
			if (expr == "true")
			{
				return true;
			}
			if (expr == "false")
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// 分割函数参数
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static string[] SplitArguments(string expr)
		{
			var args = new List<string>();
			var index = 0;
			var quotes = new char[]
			{
				'"', '\''
			};
			var startQuotes = false;
			for (int i = 0; i < expr.Length; i++)
			{
				var item = expr[i];
				if (quotes.Contains(item))
				{
					if (!startQuotes)
					{
						index = i;
						startQuotes = true;
					}
					else if (i > 0 && expr[i - 1] != '\\')
					{
						startQuotes = false;
					}
				}
				if ((!startQuotes && item == ','))
				{
					args.Add(expr.Substring(index, i - index));
					index = i + 1;
				}
				if (i == expr.Length - 1)
				{
					args.Add(expr.Substring(index, i - index + 1));
				}
			}
			return args.Select(s => s.Trim()).ToArray();
		}

		/// <summary>
		/// 处理括号运算
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="match"></param>
		/// <returns></returns>
		public static bool TryIncludeToken(string expr, out Match match)
		{
			match = Regex.Match(expr, @"\((?<expr>.+)\)");
			return match.Success;
		}

		/// <summary>
		/// 处理逻辑非
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="match"></param>
		/// <returns></returns>
		public static bool TryUnaryToken(string expr, out Match match)
		{
			match = Regex.Match(expr, @"\!(?<expr1>\w+|\w+\.\w+|#\{\d+\})");
			return match.Success;
		}

		/// <summary>
		/// 二元运算
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="math"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static bool TryBinaryToken(string expr, out Match math)
		{
			var args = new List<string>
			{
				 @"\*|/|%",
				 @"\+|\-",
				 @">|<|>=|<=",
				 @"==|!=",
				 @"&&",
				 @"\|\|"
			};
			foreach (var item in args)
			{
				var pattern = $@"(?<expr1>(\w+|\w+\.\w+|#\{{\d+\}}))\s*(?<expr2>({item}))\s*(?<expr3>(\w+|\w+\.\w+|#\{{\d+\}}))";
				math = Regex.Match(expr, pattern);
				if (math.Success)
				{
					return true;
				}
			}
			math = null;
			return false;
		}


		/// <summary>
		/// 匹配函数调用
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="match"></param>
		/// <returns></returns>
		public static bool TryMethodCallToken(string expr, out Match match)
		{
			match = Regex.Match(expr, @"((?<type>\w+\.)*)(?<name>\w+)\((?<args>[^\(|\)]+)\)");
			return match.Success;
		}
	}
}
