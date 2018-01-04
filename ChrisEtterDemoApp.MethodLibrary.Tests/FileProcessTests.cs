using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace ChrisEtterDemoApp.MethodLibrary.Tests
{
    [TestClass]
    public class FileProcessTests
    {
        #region Constants
        private const string BAD_FILE_NAME = @"C:\NotExists.bad";
        private const string FILE_NAME = @"FileToDeploy.txt";
        #endregion

        #region Properties
        private string _GoodFileName;
        public TestContext TestContext { get; set; }
        #endregion

        #region Initialization / Cleanup Methods
        [ClassInitialize]
        public static void ClassInitialize(TestContext tc)
        {
            // TODO: Initialize for all tests within a class
            tc.WriteLine("In ClassInitialize");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // TODO: Clean up after all tests within this class
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestContext.WriteLine("In TestInitialize");

            _GoodFileName = FILE_NAME;

            if (TestContext.TestName == "FileNameDoesExist")
            {
                if (!string.IsNullOrEmpty(_GoodFileName))
                {
                    TestContext.WriteLine("Creating file: " + _GoodFileName);
                    // Create the 'Good' file.
                    File.AppendAllText(_GoodFileName, "Some Text");
                }
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TestContext.WriteLine("In TestCleanup");

            if (TestContext.TestName == "FileNameDoesExist")
            {
                // Delete file
                if (File.Exists(_GoodFileName))
                {
                    TestContext.WriteLine("Deleting file: " + _GoodFileName);
                    File.Delete(_GoodFileName);
                }
            }
        }
        #endregion

        [TestMethod]
        [Owner("ChrisEtter")]
        [Priority(0)]
        [TestCategory("NoException")]
        [Description("Check to see if a file exists.")]
        public void FileNameDoesExist()
        {
            FileProcess fp = new FileProcess();
            bool fromCall;

            TestContext.WriteLine("Checking file: " + _GoodFileName);
            fromCall = fp.FileExists(_GoodFileName);

            Assert.IsTrue(fromCall);
        }

        [TestMethod]
        [Owner("ChrisEtter")]
        [DeploymentItem(FILE_NAME)]
        public void FileNameDoesExistUsingDeploymentItem()
        {
            FileProcess fp = new FileProcess();
            string fileName;
            bool fromCall;

            fileName = Directory.GetCurrentDirectory() + "\\" + FILE_NAME;
            TestContext.WriteLine("Checking file: " + fileName);

            fromCall = fp.FileExists(fileName);

            Assert.IsTrue(fromCall);
        }

        [TestMethod]
        [Owner("ChrisEtter")]
        [Priority(1)]
        [TestCategory("NoException")]
        [Description("Check to see if file does not exist.")]
        public void FileNameDoesNotExist()
        {
            FileProcess fp = new FileProcess();
            bool fromCall;

            TestContext.WriteLine("Checking file: " + BAD_FILE_NAME);
            fromCall = fp.FileExists(BAD_FILE_NAME);

            Assert.IsFalse(fromCall);
        }

        [TestMethod]
        [Owner("ChrisEtter")]
        [Priority(0)]
        [TestCategory("Exception")]
        [Description("Check for a thrown ArgumentNullException.")]
        public void FileNameNullOrEmpty_ThrowsArgumentNullException()
        {
            FileProcess fp = new FileProcess();

            try
            {
                fp.FileExists("");
            }
            catch (ArgumentNullException)
            {
                // Test was a success
                return;
            }

            // Fail the test
            Assert.Fail("Call to FileExists() did NOT throw an ArgumentNullException.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [Owner("ChrisEtter")]
        [Priority(1)]
        [TestCategory("Exception")]
        [Description("Check for a thrown ArgumentNullException using ExpectedException.")]
        public void FileNameNullOrEmpty_ThrowsArgumentNullException_UsingAttribute()
        {
            FileProcess fp = new FileProcess();

            fp.FileExists("");
        }

        [TestMethod]
        [Timeout(3000)]
        public void SimulateTimeout()
        {
            System.Threading.Thread.Sleep(2000);
        }
    }
}
