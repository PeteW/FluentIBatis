using System;
using System.Reflection;

namespace FluentIbatis.Core.Contracts
{
    public interface IPropertyMappingProvider
    {
        PropertyMapping GetPropertyMapping();
    }
}