using RpcLikeBlazor.ApiAttributes;
using RpcLikeBlazor.ApiServiceSetup.Abstractions;
using RpcLikeBlazor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RpcLikeBlazor.ApiServiceSetup.Implementations.ApiInterfacesSources
{
    /// <summary>
    /// Find all the Api Interfaces by <see cref="ApiInterfaceAttribute"/>.
    /// </summary>
    public class ApiInterfaceAttributeSource : IApiInterfacesSource
    {
        /// <inheritdoc/>
        public IEnumerable<Type> ApiInterfaces { get; }

        /// <summary>
        /// Constructor for <see cref="ApiInterfaceAttribute"/>.
        /// </summary>
        /// <param name="assembly">Assembly where to find Api Interfaces.</param>
        public ApiInterfaceAttributeSource(Assembly assembly)
        {
            ArgumentsHelpers.ThrowIfNull(assembly, nameof(assembly));
            ApiInterfaces = assembly.DefinedTypes
                .Where(t => t.GetCustomAttribute<ApiInterfaceAttribute>() != null)
                .ToList();
        }
    }
}
