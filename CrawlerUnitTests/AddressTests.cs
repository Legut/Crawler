using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CrawlerUnitTests
{
    [TestClass]
    public class AddressTests
    {
        [TestMethod]
        public void TestAddressWithoutSSL()
        {
            // Arrange
            Uri baseUrl = new Uri("https://example.com");
            string address = "http://example.com/subpage";
            string pfAddress = "https://example.com/test";
            string expected = "http://example.com/subpage";

            // Act
            Crawler.Base.Crawler.NormalizeAddress(baseUrl, ref address, pfAddress);

            // Assert
            string actual = address;
            Assert.AreEqual(expected, actual, true, "Addres normalized incorrectly");
        }

        [TestMethod]
        public void TestAddressWithoutProtocol()
        {
            // Arrange
            Uri baseUrl = new Uri("https://example.com");
            string address = "example.com/subpage";
            string pfAddress = "https://example.com/test";
            string expected = "http://example.com/subpage";

            // Act
            Crawler.Base.Crawler.NormalizeAddress(baseUrl, ref address, pfAddress);

            // Assert
            string actual = address;
            Assert.AreEqual(expected, actual, true, "Addres normalized incorrectly");
        }

        [TestMethod]
        public void TestAbsoluteAddress()
        {
            // Arrange
            Uri baseUrl = new Uri("https://example.com");
            string address = "/subpage/to/somewhere";
            string pfAddress = "https://example.com/test";
            string expected = "https://example.com/subpage/to/somewhere";

            // Act
            Crawler.Base.Crawler.NormalizeAddress(baseUrl, ref address, pfAddress);

            // Assert
            string actual = address;
            Assert.AreEqual(expected, actual, true, "Addres normalized incorrectly");
        }

        [TestMethod]
        public void TestRelativeAddress()
        {
            // Arrange
            Uri baseUrl = new Uri("https://example.com");
            string address = "subpage/to/somwhere";
            string pfAddress = "https://example.com/test/that";
            string expected = "https://example.com/test/subpage/to/somwhere";

            // Act
            Crawler.Base.Crawler.NormalizeAddress(baseUrl, ref address, pfAddress);

            // Assert
            string actual = address;
            Assert.AreEqual(expected, actual, true, "Addres normalized incorrectly");
        }

        [TestMethod]
        public void TestRelativeAddressWithImage()
        {
            // Arrange
            Uri baseUrl = new Uri("https://example.com");
            string address = "subpage/to/somwhere.png";
            string pfAddress = "https://example.com/test/that";
            string expected = "https://example.com/test/subpage/to/somwhere.png";

            // Act
            Crawler.Base.Crawler.NormalizeAddress(baseUrl, ref address, pfAddress);

            // Assert
            string actual = address;
            Assert.AreEqual(expected, actual, true, "Addres normalized incorrectly");
        }

        [TestMethod]
        public void TestDoubleSlashAddress()
        {
            // Arrange
            Uri baseUrl = new Uri("https://example.com");
            string address = "//example.com/subpage/to/somwhere";
            string pfAddress = "https://example.com/test/that";
            string expected = "http://example.com/subpage/to/somwhere";

            // Act
            Crawler.Base.Crawler.NormalizeAddress(baseUrl, ref address, pfAddress);

            // Assert
            string actual = address;
            Assert.AreEqual(expected, actual, true, "Addres normalized incorrectly");
        }

        [TestMethod]
        public void TestHashtagAddress()
        {
            // Arrange
            Uri baseUrl = new Uri("https://example.com");
            string address = "#somwhere";
            string pfAddress = "https://example.com/test/that";
            string expected = null;

            // Act
            Crawler.Base.Crawler.NormalizeAddress(baseUrl, ref address, pfAddress);

            // Assert
            string actual = address;
            Assert.AreEqual(expected, actual, true, "Addres normalized incorrectly");
        }

        [TestMethod]
        public void TestTelephoneAddress()
        {
            // Arrange
            Uri baseUrl = new Uri("https://example.com");
            string address = "tel:123123123";
            string pfAddress = "https://example.com/test/that";
            string expected = null;

            // Act
            Crawler.Base.Crawler.NormalizeAddress(baseUrl, ref address, pfAddress);

            // Assert
            string actual = address;
            Assert.AreEqual(expected, actual, true, "Addres normalized incorrectly");
        }

        [TestMethod]
        public void TestNullAddress()
        {
            // Arrange
            Uri baseUrl = new Uri("https://example.com");
            string address = null;
            string pfAddress = "https://example.com/test/that";
            string expected = null;

            // Act
            Crawler.Base.Crawler.NormalizeAddress(baseUrl, ref address, pfAddress);

            // Assert
            string actual = address;
            Assert.AreEqual(expected, actual, true, "Addres normalized incorrectly");
        }

        [TestMethod]
        public void TestEmptyAddress()
        {
            // Arrange
            Uri baseUrl = new Uri("https://example.com");
            string address = String.Empty;
            string pfAddress = "https://example.com/test/that";
            string expected = null;

            // Act
            Crawler.Base.Crawler.NormalizeAddress(baseUrl, ref address, pfAddress);

            // Assert
            string actual = address;
            Assert.AreEqual(expected, actual, true, "Addres normalized incorrectly");
        }
    }
}
