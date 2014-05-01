using System;
using System.Collections.Generic;
using System.Data;
using FluentIbatis.Core;
using NUnit.Framework;

namespace FluentIbatis.UnitTests
{
    [TestFixture]
    public class PresentationTests
    {
        /// <summary>
        /// dummy class
        /// </summary>
        public class Employee
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public DateTime LuckyDay { get; set; }
            public object UnmappedObject { get; set; }
        }

        public class DataSources
        {
            public static DataTable GetBasicValidDataTable()
            {
                var result = new DataTable();
                result.Columns.Add("ID", typeof(int));
                result.Columns.Add("Name", typeof(string));
                result.Columns.Add("LuckyDay", typeof(DateTime));
                result.Rows.Add(1, "Pete", DateTime.Now);
                result.Rows.Add(2, "Dave", DateTime.Now);
                result.Rows.Add(3, "Jeff", DateTime.Now);
                return result;
            }
        }

        /// <summary>
        /// mapping for the dummy class
        /// </summary>
        public class BasicValidEmployeeClassMap : ClassMap<Employee>
        {
            public BasicValidEmployeeClassMap()
            {
                Map(x => x.ID, "ID");
                Map(x => x.Name);
                Map(x => x.LuckyDay);
            }
        }

        [Test]
        public void TestBasicFunctionality()
        {
            DataTable dataTable = DataSources.GetBasicValidDataTable();
            IList<Employee> employees = ObjectBuilder.Map(new BasicValidEmployeeClassMap(), dataTable);
            Assert.AreEqual(employees.Count, 3);
            Assert.AreEqual(employees[0].Name, "Pete");
            Assert.AreEqual(employees[0].ID, 1);
            Assert.AreEqual(employees[0].LuckyDay.Date, DateTime.Now.Date);
            Assert.AreEqual(employees[0].UnmappedObject, null);
        }
    }
}