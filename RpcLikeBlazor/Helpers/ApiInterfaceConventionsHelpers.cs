using RpcLikeBlazor.ApiAttributes;
using RpcLikeBlazor.ApiServiceSetup;
using System;
using System.Linq;
using System.Reflection;

namespace RpcLikeBlazor.Helpers
{
    internal static class ApiInterfaceConventionsHelpers
    {
        public static string GetRouteByInterfaceName(string name)
        {
            if (name.StartsWith('I'))
            {
                name = name.Remove(0, 1);
            }
            if (name.EndsWith("Api"))
            {
                name = name[0..^3];
            }
            return name;
        }

        public static bool CheckForConventions(Type apiInterface, ApiServiceSettings settings)
        {
            var throwException = settings.OnApiInterfaceConventionViolated == OnApiInterfaceConventionViolated.ThrowException;

            return CheckForInterfaceType(apiInterface, throwException)
                && CheckForMaxOneComplexParameter(apiInterface, throwException)
                && CheckForNameDifference(apiInterface, throwException);
        }

        private static bool CheckForInterfaceType(Type apiInterface, bool throwException)
        {
            if (!apiInterface.IsInterface)
            {
                if (!throwException)
                {
                    return false;
                }
                throw new Exceptions.ApiConventionExceptions.NotInterfaceException(apiInterface);
            }
            return true;
        }

        private static bool CheckForMaxOneComplexParameter(Type apiInterface, bool throwException)
        {
            foreach (var method in apiInterface.GetMethods())
            {
                var complexParameters = method.GetParameters()
                    .Where(p => !TypeHelpers.IsSimpleType(p.ParameterType)).ToList();
                if (complexParameters.Count > 1)
                {
                    if (!throwException)
                    {
                        return false;
                    }
                    throw new Exceptions.ApiConventionExceptions.FewComplexParametersException(apiInterface)
                    {
                        ApiInterfaceMethod = method,
                        ComplexParameters = complexParameters,
                    };
                }
            }
            return true;
        }

        private static bool CheckForNameDifference(Type apiInterface, bool throwException)
        {
            var duplicatedNames = apiInterface.GetMethods()
                .GroupBy(m => new { m.Name, m.GetCustomAttribute<ApiHttpMethodAttribute>()?.HttpMethod.Method })
                .Where(g => g.Count() > 1)
                .Select(g => g.Key.Name)
                .ToList();
            if (duplicatedNames.Any())
            {
                if (!throwException)
                {
                    return false;
                }
                throw new Exceptions.ApiConventionExceptions.DuplicatedNamesException(apiInterface)
                {
                    DuplicatedMethodsNames = duplicatedNames,
                };
            }
            return true;
        }
    }
}
