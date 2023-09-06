using System.Linq.Expressions;

namespace Soul.Expressions.Test
{
    public class P
    {
        public string Name { get; set; }
    }
    [TestClass]
    public class UnitTest1
    {
        [TestMethod("�����Զ�����ת��")]
        public void AutoTypeCast()
        {
            var compiler = new SyntaxCompiler();
            //����
            var labmda = compiler.Lambda("2.0 + a", typeof(double?), new Parameter("a", typeof(int?)));
            var func = labmda.Compile() as Func<int?, double?>;
            var result = func(2);
            Console.WriteLine(func(2));
        }
    }
}