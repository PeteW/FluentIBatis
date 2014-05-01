using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace FluentIbatis.Core
{
    public class DBAction<T> where T:new()
    {
        private string functionName;
        private ClassMap<T> mapping;
        private IDictionary<string, object> parameters = new Dictionary<string, object>();
        /// <summary>
        /// Initializes a new instance of the <see cref="DBAction&lt;T&gt;"/> class.
        /// </summary>
        private DBAction() {}
        /// <summary>
        /// Creates the DBAction.
        /// </summary>
        /// <param name="functionName">The call.</param>
        /// <returns></returns>
        public static DBAction<T> CreateCall(string functionName)
        {
            return new DBAction<T>() {functionName = functionName};
        }
        public DBAction<T> WithMapping(ClassMap<T> mapping)
        {
            this.mapping = mapping;
            return this;
        }
        public DBAction<T> WithMapping(IEnumerable<Expression<Func<T, object>>> mappings)
        {
            this.mapping = new ClassMap<T>();
            foreach (var expression in mappings)
            {
                this.mapping.Map(expression);
            }
            return this;
        }
        public DBAction<T> WithParamaters(IDictionary<string,object> paramaters)
        {
            this.parameters = paramaters;
            return this;
        }
        public T FirstOrDefault(SqlConnection connection)
        {
            return ObjectBuilder.Map<T>(mapping, GetData(connection)).FirstOrDefault();
        }
        public T First(SqlConnection connection)
        {
            return ObjectBuilder.Map<T>(mapping, GetData(connection)).First();
        }
        public IList<T> GetAll(SqlConnection connection)
        {
            return ObjectBuilder.Map<T>(mapping, GetData(connection));
        }
        private DataTable GetData(SqlConnection connection)
        {
            return SqlHandler.RunQuerySprocDataTable(functionName, parameters, connection);
        }
    }
}