using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Project.V14.Lib;

namespace Project.V14.Test
{
    [TestClass]
    public class DataServiceTest
    {
        DataService ds = new DataService();
        string filePath = @"C:\Users\Admin\source\repos\C#\Tyuiu.SysoevDA.Sprint7\данные.csv";

        [TestMethod]
        public void СountRows()
        {
            Assert.AreEqual(5, ds.СountRows(filePath));
        }

        [TestMethod]
        public void СountColumn()
        {
            Assert.AreEqual(6, ds.CountColumns(filePath));
        }

        [TestMethod]
        public void AverageTime()
        {
            Assert.AreEqual(53, ds.AverageTime(filePath));
        }
        [TestMethod]
        public void MinTime()
        {
            Assert.AreEqual(40, ds.MinTime(filePath));
        }
        [TestMethod]
        public void MaxTime()
        {
            Assert.AreEqual(80, ds.MaxTime(filePath));
        }
    }
}
