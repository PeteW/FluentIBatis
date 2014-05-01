using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentIbatis.Core.Contracts;
using FluentIbatis.Core.Utils;

namespace FluentIbatis.Core
{
    public class ClassMap<T> where T : new()
    {
        public readonly IList<IPropertyMappingProvider> Properties = new List<IPropertyMappingProvider>();
        public PropertyPart Map(Expression<Func<T, object>> expression)
        {
            return Map(expression, null);
        }

        public PropertyPart Map(Expression<Func<T, object>> expression, string columnName)
        {
            return Map(ReflectionHelper.GetProperty(expression), columnName);
        }

        protected virtual PropertyPart Map(PropertyInfo property, string columnName)
        {
            var propertyMap = new PropertyPart(property, typeof(T));

            if (!string.IsNullOrEmpty(columnName))
                propertyMap.SetColumn(columnName);
            Properties.Add(propertyMap);

            return propertyMap;
        }
        public Type EntityType
        {
            get { return typeof(T); }
        }
    }
}