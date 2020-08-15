using RpcLikeBlazor.ApiAttributes;
using RpcLikeBlazor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RpcLikeBlazor.ApiServiceSetup
{
    internal static class ApiConventionManager
    {
        private static readonly IList<Type> conventionCheckPassedTypes = new List<Type>();

        public static bool CheckForConventions(Type apiInterface, ApiServiceSettings settings)
        {
            if (conventionCheckPassedTypes.Contains(apiInterface))
            {
                return true;
            }

            var throwException = (settings?.OnApiInterfaceConventionViolated ?? OnApiInterfaceConventionViolated.SkipAndContinueRunning)
                == OnApiInterfaceConventionViolated.ThrowException;

            var success = CheckForInterfaceType(apiInterface, throwException)
                && CheckForMaxOneComplexParameter(apiInterface, throwException)
                && CheckForNameDifference(apiInterface, throwException);

            if (success)
            {
                conventionCheckPassedTypes.Add(apiInterface);
            }
            return success;
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
