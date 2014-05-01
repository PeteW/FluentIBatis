using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FluentIbatis.Core
{
    public class SqlHandler
    {
        public static DataTable RunQuerySprocDataTable(string sprocName, IDictionary<string, object> parameterNamesAndValues, SqlConnection sqlConnection)
        {
            return RunQuerySprocDataTable(sprocName, parameterNamesAndValues.Keys.ToList(), parameterNamesAndValues.Values.ToList(), sqlConnection);
        }

        /// <summary>
        /// Runs the query stored procedure, returns a datatable.
        /// </summary>
        /// <param name="sprocName">Name of the sproc.</param>
        /// <param name="parameterNames">The parameter names.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        public static DataTable RunQuerySprocDataTable(string sprocName, List<string> parameterNames, List<object> parameterValues, SqlConnection sqlConnection)
        {
            var dataTable = new DataTable();
            var sqlCommand = new SqlCommand(sprocName, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 300;
            for (int i = 0; i < parameterNames.Count; i++)
            {
                sqlCommand.Parameters.AddWithValue(parameterNames[i], parameterValues[i]);
            }
            var sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
    }
}