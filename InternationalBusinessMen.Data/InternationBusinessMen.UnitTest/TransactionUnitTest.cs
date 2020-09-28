using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
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
    public class TransactionUnitTest
    {
        private Mock<DataModel> _mockDataModel;
        private Mock<IConversionService> _conversionService;

        [SetUp]
        public void SetUp()
        {
            _mockDataModel = new Mock<DataModel>();
            _conversionService = new Mock<IConversionService>();
        }

        [Test]
        public void GetAllTransactionsTest()
        {
            var mockDataSet = new Mock<DbSet<Transaction>>();
            _mockDataModel.Setup(dm => dm.Transactions).Returns(CreateTransactions());

            var result = GetTransactionService().RetrieveSaveAllTransactions();

            Assert.IsTrue(result.Count() == 3);
        }

        [Test]
        public void NotNullResultTest()
        {
            var mockDataSet = new Mock<DbSet<Transaction>>();
            _mockDataModel.Setup(dm => dm.Transactions).Returns(CreateTransactions());

            var result = GetTransactionService().RetrieveSaveAllTransactions();

            Assert.IsNotNull(result);
        }

        [Test]
        public void NotNullItemResultTest()
        {
            var mockDataSet = new Mock<DbSet<Transaction>>();
            _mockDataModel.Setup(dm => dm.Transactions).Returns(CreateTransactions());

            var result = GetTransactionService().RetrieveSaveAllTransactions();

            Assert.IsNotNull(result[0]);
            Assert.IsNotNull(result[1]);
            Assert.IsNotNull(result[2]);
        }

        [Test]
        public void NotNegativeItemResultTest()
        {
            var mockDataSet = new Mock<DbSet<Transaction>>();
            _mockDataModel.Setup(dm => dm.Transactions).Returns(CreateTransactions());

            var result = GetTransactionService().RetrieveSaveAllTransactions();

            Assert.IsTrue(result[0].Amount > 0);
            Assert.IsTrue(result[1].Amount > 0);
            Assert.IsTrue(result[2].Amount > 0);
        }

        [Test]
        public void HasSkuTest()
        {
            var mockDataSet = new Mock<DbSet<Transaction>>();
            _mockDataModel.Setup(dm => dm.Transactions).Returns(CreateTransactions());

            var result = GetTransactionService().RetrieveSaveAllTransactions();

            Assert.IsNotEmpty(result[0].Sku);
            Assert.IsNotEmpty(result[1].Sku);
            Assert.IsNotEmpty(result[2].Sku);
        }

        [Test]
        public void HasCurrencyTest()
        {
            var mockDataSet = new Mock<DbSet<Transaction>>();
            _mockDataModel.Setup(dm => dm.Transactions).Returns(CreateTransactions());

            var result = GetTransactionService().RetrieveSaveAllTransactions();

            Assert.IsNotEmpty(result[0].Currency);
            Assert.IsNotEmpty(result[1].Currency);
            Assert.IsNotEmpty(result[2].Currency);
        }

        [Test]
        public void GetExternalTransactionsTest()
        {
            var mockDataSet = new Mock<DbSet<Transaction>>();
            _mockDataModel.Setup(dm => dm.Transactions).Returns(CreateTransactions());

            var result = GetTransactionService().RetrieveSaveAllTransactions();

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetExternalTransactionsBySku()
        {
            var mockDataSet = new Mock<DbSet<Transaction>>();
            _mockDataModel.Setup(dm => dm.Transactions).Returns(CreateTransactions());
            _conversionService.Setup(cs => cs.ConvertToEUR(CreateTransactions().Where(t => t.Sku == "TESTX").ToList()))
                .Returns(new List<Transaction>() { new Transaction
                {
                    Sku = "TESTX",
                    Currency = "AUD",
                    Amount = 10.9M,
                    EURAmount = 8.7M
                }});

            var result = GetTransactionService().GetTransactionsBySku("TESTX");

            Assert.IsNotNull(result);
        }

        public DbSet<Transaction> CreateTransactions()
        {
            var allTransactions = new List<Transaction>
            {
                new Transaction
                {
                    TransactionID = 1,
                    Amount = 10.9M,
                    Currency = "AUD",
                    Sku = "TESTX"
                },
                new Transaction
                {
                    TransactionID = 2,
                    Amount = 15.5M,
                    Currency = "EUR",
                    Sku = "TESTY"
                },
                new Transaction
                {
                    TransactionID = 3,
                    Amount = 30.1M,
                    Currency = "CAD",
                    Sku = "TESTZ"
                }
            }.GetQueryableMockDbSet();

            return allTransactions;
        }

        private TransactionService GetTransactionService()
        {
            return new TransactionService(_mockDataModel.Object, _conversionService.Object);
        }
    }
}
