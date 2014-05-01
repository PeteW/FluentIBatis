using System;

namespace FluentIbatis.Core.Utils
{
    public class ClassMappingException:Exception
    {
        public ClassMappingException(string message) : base(message) {}
    }
}