﻿using System.Linq.Expressions;

namespace Soul.Expressions.Test
{
	public class C
	{
		public string Name { get; set; }
	}

	internal class Program
	{
		static void Main(string[] args)
		{
			Expression<Func<string, bool>> expr = s => string.IsNullOrEmpty(s);
			var syntax = SyntaxEngine.Run("string.IsNullOrEmpty(p.Name)", new Parameter("p", typeof(C)));
			Console.WriteLine(syntax.Debug);
			SyntaxCompiler.RegisterStaticMethods(typeof(Funcs));
			var expression = SyntaxCompiler.Lambda(syntax);
			var func = expression.Compile();
			Console.WriteLine(func.DynamicInvoke(new C { Name = "aa" }));
			Test();
		}

		public static void Test()
		{
			var tree0 = SyntaxEngine.Run("!flag && 1 > 2", new Parameter("flag", typeof(bool)));
			var expression0 = SyntaxCompiler.Lambda(tree0);
			Console.WriteLine(tree0.Debug);
			var tree1 = SyntaxEngine.Run("(1 + 2) * 4 / 5");
			var expression1 = SyntaxCompiler.Lambda(tree1);
			Console.WriteLine(tree1.Debug);
			var tree2 = SyntaxEngine.Run("Pow(2, 2) + 2");
			var expression2 = SyntaxCompiler.Lambda(tree2);
			Console.WriteLine(tree2.Debug);
			var tree3 = SyntaxEngine.Run("a >= 2 && 1 > 0", new Parameter("a", typeof(int)));
			var expression3 = SyntaxCompiler.Lambda(tree3);
			Console.WriteLine(tree3.Debug);
			var tree4 = SyntaxEngine.Run("a > 2 && true", new Parameter("a", typeof(int)));
			var expression4 = SyntaxCompiler.Lambda(tree4);
			Console.WriteLine(tree4.Debug);
		}
	}
}