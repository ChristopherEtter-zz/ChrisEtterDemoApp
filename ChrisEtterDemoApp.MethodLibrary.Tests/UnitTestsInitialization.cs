using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChrisEtterDemoApp.MethodLibrary.Tests
{
    [TestClass]
    public class UnitTestsInitialization
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext tc)
        {
            // TODO: Initialize for all tests within an assembly
            tc.WriteLine("In AssemblyInitialize");
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            // TODO: Clean up after all tests in an assembly
        }
    }
}