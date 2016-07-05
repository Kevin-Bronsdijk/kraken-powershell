using System;
using Kraken.Powershell;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class HelperFunctionTests
    {
        [TestMethod]
        public void DestinationIsSameAzureContainer_NotOnAzure_IsTrue()
        {
            var url = new Uri("https://devslice.net/folder1/folder2/image.png");

            Assert.IsTrue(HelperFunctions.DestinationUsesSameAzureContainer(url, "testContainer") != true);
        }

        [TestMethod]
        public void DestinationIsSameAzureContainer_NotOnAzureSameRootFolder_IsTrue()
        {
            var url = new Uri("https://devslice.net/testContainer/folder2/image.png");

            Assert.IsTrue(HelperFunctions.DestinationUsesSameAzureContainer(url, "testContainer") != true);
        }

        [TestMethod]
        public void DestinationIsSameAzureContainer_OnAzureDifferentRootFolder_IsTrue()
        {
            var url = new Uri("https://kraken.blob.core.windows.net/test/folder2/image.png");

            Assert.IsTrue(HelperFunctions.DestinationUsesSameAzureContainer(url, "testContainer") != true);
        }

        [TestMethod]
        public void DestinationIsSameAzureContainer_OnAzureSameRootFolder_IsTrue()
        {
            var url = new Uri("https://kraken.blob.core.windows.net/testContainer/folder2/image.png");

            Assert.IsTrue(HelperFunctions.DestinationUsesSameAzureContainer(url, "testContainer"));
        }

        [TestMethod]
        public void BuildAzurePath_SourceNotAzureNewPathWithCustomPath_IsTrue()
        {
            const string url = "https://devslice.net/folder1/folder2/image.png";
            bool keepPath = false;

            var result = HelperFunctions.BuildPathAzure(url, keepPath, "powershell", "test");

            Assert.IsTrue(result == "/powershell/image.png");
        }

        [TestMethod]
        public void BuildAzurePath_SourceNotAzureNewPathNoCustomPath_IsTrue()
        {
            const string url = "https://devslice.net/folder1/folder2/image.png";
            bool keepPath = false;

            var result = HelperFunctions.BuildPathAzure(url, keepPath, "", "test");

            Assert.IsTrue(result == "/image.png");
        }

        [TestMethod]
        public void BuildAzurePath_SourceNotAzureNewPathKeepOldPathNoCustomPath_IsTrue()
        {
            const string url = "https://devslice.net/folder1/folder2/image.png";
            bool keepPath = true;

            var result = HelperFunctions.BuildPathAzure(url, keepPath, "", "folder1");

            Assert.IsTrue(result == "/folder1/folder2/image.png");
        }


        [TestMethod]
        public void BuildAzurePath_SourceAzureNewPathWithCustomPath_IsTrue()
        {
            const string url = "https://kraken.blob.core.windows.net/folder1/folder2/image.png";
            bool keepPath = false;

            var result = HelperFunctions.BuildPathAzure(url, keepPath, "powershell", "test");

            Assert.IsTrue(result == "/powershell/image.png");
        }

        [TestMethod]
        public void BuildAzurePath_SourceAzureNewPathNoCustomPath_IsTrue()
        {
            const string url = "https://kraken.blob.core.windows.net/folder1/folder2/image.png";
            bool keepPath = false;

            var result = HelperFunctions.BuildPathAzure(url, keepPath, "", "test");

            Assert.IsTrue(result == "/image.png");
        }

        [TestMethod]
        public void BuildAzurePath_SourceAzureNewPathKeepOldPathNoCustomPath_IsTrue()
        {
            const string url = "https://kraken.blob.core.windows.net/folder1/folder2/image.png";
            bool keepPath = true;

            var result = HelperFunctions.BuildPathAzure(url, keepPath, "", "folder1");

            Assert.IsTrue(result == "/folder2/image.png");
        }

        [TestMethod]
        public void BuildS3Path_WithCustomPath_IsTrue()
        {
            const string url = "https://devslice.net/folder1/folder2/image.png";
            bool keepPath = false;

            var result = HelperFunctions.BuildPathS3(url, keepPath, "powershell", "test");

            Assert.IsTrue(result == "powershell/image.png");
        }

        [TestMethod]
        public void BuildS3Path_NoCustomPath_IsTrue()
        {
            const string url = "https://devslice.net/folder1/folder2/image.png";
            bool keepPath = false;

            var result = HelperFunctions.BuildPathS3(url, keepPath, "", "test");

            Assert.IsTrue(result == "image.png");
        }

        [TestMethod]
        public void BuildS3Path_KeepOldPathNoCustomPath_IsTrue()
        {
            const string url = "https://devslice.net/folder1/folder2/image.png";
            bool keepPath = true;

            var result = HelperFunctions.BuildPathS3(url, keepPath, "", "folder1");

            Assert.IsTrue(result == "folder1/folder2/image.png");
        }
    }
}