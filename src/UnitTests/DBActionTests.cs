using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq.Expressions;
using FluentIbatis.Core;
using NUnit.Framework;

namespace FluentIbatis.UnitTests
{
    [TestFixture]
    public class DBActionTests
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
        [Test]
        public void TestBasicFunctionality()
        {
            var connnectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            using (var conn = new SqlConnection(connnectionString))
            {
                IList<Employee> employees = DBAction<Employee>
                    .CreateCall("spGetEmployees")
                    .WithMapping(
                        new List<Expression<Func<Employee, object>>>
                            {
                                x => x.ID,
                                x => x.Name,
                                x => x.LuckyDay
                            }
                    ).GetAll(conn);
            }
        }
    }
}