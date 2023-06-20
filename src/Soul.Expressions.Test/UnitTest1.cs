using Soul.Expressions.Utilities;

namespace Soul.Expressions.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod("�����Զ�����ת��")]
        public void AutoTypeCast()
        {
            var options = new SyntaxOptions();
            options.RegisterFunction(typeof(Functions));
            var expr = "Pow(1.0, 2)";
            var compiler = new SyntaxCompiler(options);
            var labmda = compiler.Lambda(expr);
            var result = labmda.Compile().DynamicInvoke();
            Assert.AreEqual(result, 2.2);
        }
    }
}