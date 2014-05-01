using System;
using System.Collections.Generic;
using System.Reflection;
using FluentIbatis.Core.Contracts;

namespace FluentIbatis.Core
{
    public class PropertyPart:IPropertyMappingProvider
    {
        private readonly PropertyInfo property;
        private readonly Type parentType;
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyPart"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="parentType">Type of the parent.</param>
        public PropertyPart(PropertyInfo property, Type parentType)
        {
            this.property = property;
            this.parentType = parentType;
        }
        public PropertyPart SetColumn(string columnName)
        {
            Column = columnName;
            return this;
        }
        public string Column { get; set; }

        public PropertyMapping GetPropertyMapping()
        {
            return new PropertyMapping()
                       {
                           ContainingEntityType = parentType,
                           PropertyInfo = property,
                           ColumnName = string.IsNullOrEmpty(Column)?property.Name:Column
                       };
        }
    }
}