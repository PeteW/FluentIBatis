using System;
using System.Collections.Generic;
using System.Data;
using FluentIbatis.Core.Contracts;
using FluentIbatis.Core.Utils;

namespace FluentIbatis.Core
{
    public class ObjectBuilder
    {
        /// <summary>
        /// Maps the data row.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mapping">The mapping.</param>
        /// <param name="dataRow">The data row.</param>
        /// <returns></returns>
        private static T MapDataRow<T>(ClassMap<T> mapping, DataRow dataRow) where T : new()
        {
            var result = new T();
            foreach (IPropertyMappingProvider mappingProvider in mapping.Properties)
            {
                PropertyMapping propertyMapping = mappingProvider.GetPropertyMapping();
                try
                {
                    propertyMapping.PropertyInfo.SetValue(result, dataRow[propertyMapping.ColumnName], null);
                }
                catch (Exception e)
                {
                    HandleMappingException(e, propertyMapping);
                }
            }
            return result;
        }
        /// <summary>
        /// Maps the specified mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mapping">The mapping.</param>
        /// <param name="dataTable">The data table.</param>
        /// <returns></returns>
        public static IList<T> Map<T>(ClassMap<T> mapping, DataTable dataTable) where T : new()
        {
            var result = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                result.Add(MapDataRow(mapping, row));
            }
            return result;
        }
        /// <summary>
        /// Maps the specified mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mapping">The mapping.</param>
        /// <param name="dataReader">The data reader.</param>
        /// <returns></returns>
        public static IList<T> Map<T>(ClassMap<T> mapping, IDataReader dataReader) where T : new()
        {
            var result = new List<T>();
            while (dataReader.Read())
            {
                result.Add(MapDataReader(mapping, dataReader));
            }
            return result;
        }
        /// <summary>
        /// Maps the data reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mapping">The mapping.</param>
        /// <param name="dataReader">The data reader.</param>
        /// <returns></returns>
        private static T MapDataReader<T>(ClassMap<T> mapping, IDataReader dataReader) where T : new()
        {
            var result = new T();
            foreach (IPropertyMappingProvider mappingProvider in mapping.Properties)
            {
                PropertyMapping propertyMapping = mappingProvider.GetPropertyMapping();
                try
                {
                    int ordinal = dataReader.GetOrdinal(propertyMapping.ColumnName);
                    propertyMapping.PropertyInfo.SetValue(result, dataReader.GetValue(ordinal), null);
                }
                catch (Exception e)
                {
                    HandleMappingException(e, propertyMapping);
                }
            }
            return result;
        }
        /// <summary>
        /// Handles the mapping exception.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="mapping">The mapping.</param>
        private static void HandleMappingException(Exception e, PropertyMapping mapping)
        {
            throw new ClassMappingException(
                string.Format("The following exception occurred when trying to map column [{0}] to property [{1}] inside of type [{2}]. [{3}]",
                              mapping.ColumnName,
                              mapping.PropertyInfo.Name,
                              mapping.ContainingEntityType,
                              e.Message
                    )
                );
        }
    }
}