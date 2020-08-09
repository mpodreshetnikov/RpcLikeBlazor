using System;
using System.Linq;

namespace RpcLikeBlazor.Helpers
{
    internal static class TypeHelpers
    {
        public static bool IsSimpleType(Type type)
        {
            var asPrimitives = new[]
            {
                    typeof(DateTime),
                    typeof(Guid),
                    typeof(decimal),
                    typeof(string),
            };

            return type.IsPrimitive || type.IsEnum || asPrimitives.Contains(type);
        }
    }
}
