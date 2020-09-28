using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Web;
using InternationalBusinessMen.API.Interfaces;
using InternationalBusinessMen.API.Services;
using InternationalBusinessMen.Data;
using InternationalBusinessMen.Data.Model;
using InternationBusinessMen.UnitTest.Helpers;
using Moq;
using NUnit.Framework;


namespace InternationBusinessMen.UnitTest
{
    [TestFixture]
    public class ConversionUnitTest
    {
        private Mock<DataModel> _mockDataModel;

        [SetUp]
        public void SetUp()
        {
            _mockDataModel = new Mock<DataModel>();
        }

        [Test]
        public void ConversionTest()
        {
            var mockDataSet = new Mock<DbSet<Conversion>>();
            _mockDataModel.Setup(dm => dm.Conversions).Returns(CreateConversions());

            List<Transaction> transactions = CreateTransactions();

            var result = GetConversionService().ConvertToEUR(transactions).ToList();

            Assert.AreEqual(8.94M, result[0].EURAmount);
            Assert.AreEqual(15.50M, result[1].EURAmount);
            Assert.AreEqual(13.82M, result[2].EURAmount);
        }

        [Test]
        public void NoZeroResultsTest()
        {
            var mockDataSet = new Mock<DbSet<Conversion>>();
            _mockDataModel.Setup(dm => dm.Conversions).Returns(CreateConversions());

            List<Transaction> transactions = CreateTransactions();

            var result = GetConversionService().ConvertToEUR(transactions).ToList();

            Assert.AreNotEqual(0.0M, result[0].EURAmount);
            Assert.AreNotEqual(0.0M, result[1].EURAmount);
            Assert.AreNotEqual(0.0M, result[2].EURAmount);
        }

        [Test]
        public void NoNullResultsTest()
        {
            var mockDataSet = new Mock<DbSet<Conversion>>();
            _mockDataModel.Setup(dm => dm.Conversions).Returns(CreateConversions());

            List<Transaction> transactions = CreateTransactions();

            var result = GetConversionService().ConvertToEUR(transactions).ToList();

            Assert.IsNotNull(result[0]);
            Assert.IsNotNull(result[1]);
            Assert.IsNotNull(result[2]);
        }

        [Test]
        public void NoEqualResultsTest()
        {
            var mockDataSet = new Mock<DbSet<Conversion>>();
            _mockDataModel.Setup(dm => dm.Conversions).Returns(CreateConversions());

            List<Transaction> transactions = CreateTransactions();

            var result = GetConversionService().ConvertToEUR(transactions).ToList();

            Assert.AreNotEqual(result[0], result[1]);
            Assert.AreNotEqual(result[1], result[2]);
            Assert.AreNotEqual(result[2], result[0]);
        }

        [Test]
        public void GetExternalConversionsTest()
        {
            var mockDataSet = new Mock<DbSet<Conversion>>();
            _mockDataModel.Setup(dm => dm.Conversions).Returns(CreateConversions());

            var result = GetConversionService().RetrieveSaveAllConversions();

            Assert.IsNotNull(result);
        }

        public DbSet<Conversion> CreateConversions()
        {
            var testConversions = new List<Conversion>
                {
                    new Conversion
                    {
                        ConversionId = 1,
                        From = "CAD",
                        To = "AUD",
                        Rate = 0.56M
                    },
                    new Conversion
                    {
                        ConversionId = 2,
                        From = "AUD",
                        To = "CAD",
                        Rate = 1.79M
                    },
                    new Conversion
                    {
                        ConversionId = 3,
                        From = "AUD",
                        To = "EUR",
                        Rate = 0.82M
                    },
                    new Conversion
                    {
                        ConversionId = 4,
                        From = "EUR",
                        To = "AUD",
                        Rate = 1.22M
                    },
                    new Conversion
                    {
                        ConversionId = 5,
                        From = "CAD",
                        To = "USD",
                        Rate = 1.49M
                    },
                    new Conversion
                    {
                        ConversionId = 6,
                        From = "USD",
                        To = "CAD",
                        Rate = 0.67M
                    }
            }.GetQueryableMockDbSet();

            return testConversions;
        }

        public List<Transaction> CreateTransactions()
        {
            List<Transaction> allTransactions = new List<Transaction>();

            Transaction transaction1 = new Transaction()
            {
                TransactionID = 1,
                Amount = 10.9M,
                Currency = "AUD",
                Sku = "TEST1"
            };
            allTransactions.Add(transaction1);

            Transaction transaction2 = new Transaction()
            {
                TransactionID = 2,
                Amount = 15.5M,
                Currency = "EUR",
                Sku = "TEST1"
            };
            allTransactions.Add(transaction2);

            Transaction transaction3 = new Transaction()
            {
                TransactionID = 3,
                Amount = 30.1M,
                Currency = "CAD",
                Sku = "TEST1"
            };
            allTransactions.Add(transaction3);

            return allTransactions;
        }

        private ConversionService GetConversionService()
        {
            return new ConversionService(_mockDataModel.Object);
        }
    }
}
