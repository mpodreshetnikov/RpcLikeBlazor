using RpcLikeBlazor.ApiServiceSetup;
using RpcLikeBlazor.ApiServiceSetup.Abstractions;
using System.Net.Http;

namespace RpcLikeBlazor
{
    /// <summary>
    /// Service that can create an <see cref="ApiCaller{TInterface}"/> instance.
    /// </summary>
    public class ApiService
    {
        private readonly HttpClient client;
        private readonly IObjectConverter objectConverter;

        public ApiService(HttpClient client, IObjectConverter objectConverter)
        {
            this.client = client;
            this.objectConverter = objectConverter;
        }

        /// <summary>
        /// Create <see cref="ApiCaller{TInterface}"/> if conventions checks passed else returns null.
        /// </summary>
        /// <typeparam name="TInterface">Api Interface.</typeparam>
        public ApiCaller<TInterface> Of<TInterface>()
            where TInterface : class
        {
            return ApiConventionManager.CheckForConventions(typeof(TInterface), null)
                ? new ApiCaller<TInterface>(client, objectConverter)
                : null;
        }
    }
}
