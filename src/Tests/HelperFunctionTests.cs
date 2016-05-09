using System;
using kraken.powershell;
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

            Assert.IsTrue(HelperFunctions.DestinationIsSameAzureContainer(url, "testContainer") != true);
        }

        [TestMethod]
        public void DestinationIsSameAzureContainer_NotOnAzureSameRootFolder_IsTrue()
        {
            var url = new Uri("https://devslice.net/testContainer/folder2/image.png");

            Assert.IsTrue(HelperFunctions.DestinationIsSameAzureContainer(url, "testContainer") != true);
        }

        [TestMethod]
        public void DestinationIsSameAzureContainer_OnAzureDifferentRootFolder_IsTrue()
        {
            var url = new Uri("https://seamist.blob.core.windows.net/test/folder2/image.png");

            Assert.IsTrue(HelperFunctions.DestinationIsSameAzureContainer(url, "testContainer") != true);
        }

        [TestMethod]
        public void DestinationIsSameAzureContainer_OnAzureSameRootFolder_IsTrue()
        {
            var url = new Uri("https://seamist.blob.core.windows.net/testContainer/folder2/image.png");

            Assert.IsTrue(HelperFunctions.DestinationIsSameAzureContainer(url, "testContainer"));
        }


        [TestMethod]
        public void BuildAzurePath_SourceNotAzureNewPathWithAzurePath_IsTrue()
        {
            const string url = "https://devslice.net/folder1/folder2/image.png";

            var result = HelperFunctions.BuildAzurePath(url, false, "powershell", "test");

            Assert.IsTrue(result == "/powershell/image.png");
        }

        [TestMethod]
        public void BuildAzurePath_SourceNotAzureNewPathNoAzurePath_IsTrue()
        {
            const string url = "https://devslice.net/folder1/folder2/image.png";

            var result = HelperFunctions.BuildAzurePath(url, false, "", "test");

            Assert.IsTrue(result == "/image.png");
        }

        [TestMethod]
        public void BuildAzurePath_SourceNotAzureNewPathKeepOldPathNoAzurePath_IsTrue()
        {
            const string url = "https://devslice.net/folder1/folder2/image.png";

            var result = HelperFunctions.BuildAzurePath(url, true, "", "folder1");

            Assert.IsTrue(result == "/folder1/folder2/image.png");
        }


        [TestMethod]
        public void BuildAzurePath_SourceAzureNewPathWithAzurePath_IsTrue()
        {
            const string url = "https://seamist.blob.core.windows.net/folder1/folder2/image.png";

            var result = HelperFunctions.BuildAzurePath(url, false, "powershell", "test");

            Assert.IsTrue(result == "/powershell/image.png");
        }

        [TestMethod]
        public void BuildAzurePath_SourceAzureNewPathNoAzurePath_IsTrue()
        {
            const string url = "https://seamist.blob.core.windows.net/folder1/folder2/image.png";

            var result = HelperFunctions.BuildAzurePath(url, false, "", "test");

            Assert.IsTrue(result == "/image.png");
        }

        [TestMethod]
        public void BuildAzurePath_SourceAzureNewPathKeepOldPathNoAzurePath_IsTrue()
        {
            const string url = "https://seamist.blob.core.windows.net/folder1/folder2/image.png";

            var result = HelperFunctions.BuildAzurePath(url, true, "", "folder1");

            Assert.IsTrue(result == "/folder2/image.png");
        }
    }
}
