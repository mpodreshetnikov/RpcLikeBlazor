using RpcLikeBlazor.ApiServiceSetup.Abstractions;
using RpcLikeBlazor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RpcLikeBlazor.ApiServiceSetup.Implementations.ApiInterfacesSources
{
    /// <summary>
    /// Define certain Api Interfaces.
    /// </summary>
    public class ApiInterfacesTypesSource : IApiInterfacesSource
    {
        /// <inheritdoc/>
        public IEnumerable<Type> ApiInterfaces { get; }

        public ApiInterfacesTypesSource(params Type[] apiInterfaces) : this(apiInterfaces?.AsEnumerable()) { }

        public ApiInterfacesTypesSource(IEnumerable<Type> apiInterfaces)
        {
            ArgumentsHelpers.ThrowIfNull(apiInterfaces, nameof(apiInterfaces));
            ApiInterfaces = apiInterfaces.ToList();
        }
    }
}
