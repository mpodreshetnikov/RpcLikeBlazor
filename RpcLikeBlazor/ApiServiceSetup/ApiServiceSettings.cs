using RpcLikeBlazor.ApiServiceSetup.Abstractions;
using System.Collections.Generic;
using System.Net.Http;

namespace RpcLikeBlazor.ApiServiceSetup
{
    /// <summary>
    /// Behavior if convention of Api Interface is violated.
    /// </summary>
    public enum OnApiInterfaceConventionViolated
    {
        /// <summary>
        /// Throw exception.
        /// </summary>
        ThrowException,
        /// <summary>
        /// Skip this violating Api Interface.
        /// </summary>
        SkipAndContinueRunning,
    }

    /// <summary>
    /// Setting for RpcLikeBlazor services.
    /// </summary>
    public class ApiServiceSettings
    {
        /// <summary>
        ///     <para>Http Client dependency. It can be set up here or as a dependency before.</para>
        ///     <para>Only one Http Client will be accepted for an application.</para>
        ///     <para>If there is no Http Client was set up, the default Http Client will be accepted.</para>
        /// </summary>
        public HttpClient HttpClient { get; set; }

        /// <summary>
        /// Sources of an Api Interfaces.
        /// </summary>
        public IEnumerable<IApiInterfacesSource> ApiInterfacesSources { get; set; }

        /// <summary>
        /// Behavior if Api Interface violate convention.
        /// </summary>
        public OnApiInterfaceConventionViolated OnApiInterfaceConventionViolated { get; set; }
    }
}
