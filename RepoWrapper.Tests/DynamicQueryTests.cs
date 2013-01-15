using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoWrapper;
using System.Linq.Expressions;

namespace RepoWrapper.Tests
{
    [TestClass]
    public class DynamicQueryTests
    {
        [TestMethod]
        public void DynamicSinglePropertyWhereQueryReturnsExpectedSqlString()
        {
            // Arrange
            string expectedSql = "SELECT * FROM Products WHERE CategoryID = @CategoryID";

            // Actrd
            QueryResult result = DynamicQuery.GetDynamicQuery<Product>("Products", (p) => p.CategoryID == 1);

            // Assert
            Assert.AreEqual(expectedSql, result.Sql);
            Assert.AreEqual(1, result.Param.CategoryID);
        }

        [TestMethod]
        public void DynamicTworopertyWhereQueryReturnsExpectedSqlString()
        {
            // Arrange
            string expectedSql = "SELECT * FROM Products WHERE Name = @Name";

            // Actrd
            QueryResult result = DynamicQuery.GetDynamicQuery<Product>("Products", (p) => p.Name == "xbox");
            dynamic actualParam = result.Param;

            // Assert
            Assert.AreEqual(expectedSql, result.Sql);
            //Assert.AreEqual(1, actualParam.CategoryID);
            //Assert.AreEqual("xbox", actualParam.Name);
        }

        [TestMethod]
        public void DynamicMultiANDPropertyWhereQueryReturnsExpectedSqlString()
        {
            // Arrange
            string expectedSql = "SELECT * FROM Products WHERE CategoryID = @CategoryID AND Name = @Name AND Price < @Price";

            // Actrd
            QueryResult result = DynamicQuery.GetDynamicQuery<Product>("Products", (p) => p.CategoryID == 1 && p.Name == "xbox" && p.Price < 100m);

            // Assert
            Assert.AreEqual(expectedSql, result.Sql);
            Assert.AreEqual(1, result.Param.CategoryID);
            Assert.AreEqual("xbox", result.Param.Name);
            Assert.AreEqual(100, result.Param.Price);
        }

        [TestMethod]
        public void DynamicTwoORPropertyWhereQueryReturnsExpectedSqlString()
        {
            // Arrange
            string expectedSql = "SELECT * FROM Products WHERE CategoryID = @CategoryID OR Name = @Name";

            // Actrd
            QueryResult result = DynamicQuery.GetDynamicQuery<Product>("Products", (p) => p.CategoryID == 1 || p.Name == "xbox");

            // Assert
            Assert.AreEqual(expectedSql, result.Sql);
            Assert.AreEqual(1, result.Param.CategoryID);
            Assert.AreEqual("xbox", result.Param.Name);
        }

        [TestMethod]
        public void DateTimeQueryExtractsExpectedValue()
        {
            // Arrange
            string expectedSql = "SELECT * FROM Products WHERE ExpiryDate > @ExpiryDate";
            DateTime dt = DateTime.Now;

            // Actrd
            QueryResult result = DynamicQuery.GetDynamicQuery<Product>("Products", (p) => p.ExpiryDate > dt);

            // Assert
            Assert.AreEqual(expectedSql, result.Sql);
            Assert.AreEqual(dt, result.Param.ExpiryDate);
        }
    }

    internal class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int CategoryID { get; set; }
    }
}
