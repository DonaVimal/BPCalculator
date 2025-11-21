using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Parallelize(Scope = ExecutionScope.MethodLevel, Workers = 4)]

namespace BP.BDDTests
{
    [TestClass]
    public class MSTestSettings
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Runs once before all tests start
            context.WriteLine("=== BP Calculator BDD Tests Starting ===");
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            // Runs once after all tests complete
        }
    }
}
