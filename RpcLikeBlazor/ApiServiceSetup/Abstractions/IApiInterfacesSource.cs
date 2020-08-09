using System;
using System.Collections.Generic;

namespace RpcLikeBlazor.ApiServiceSetup.Abstractions
{
    /// <summary>
    /// Abstraction for Api Interface source (provide usable Api Interfaces).
    /// </summary>
    public interface IApiInterfacesSource
    {
        /// <summary>
        /// Usable Api Interfaces.
        /// </summary>
        IEnumerable<Type> ApiInterfaces { get; }
    }
}
