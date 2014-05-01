using System;
using System.Reflection;

namespace FluentIbatis.Core
{
    public class PropertyMapping
    {
        public string ColumnName { get; set; }
        public Type ContainingEntityType { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
    }
}