using RpcLikeBlazor.Core;
using System.Net.Http;

namespace RpcLikeBlazor
{
    /// <summary>
    /// Service that can create an <see cref="ApiCaller{TInterface}"/>.
    /// </summary>
    public class ApiService
    {
        private readonly HttpClient client;

        public ApiService(HttpClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Create Api Caller.
        /// </summary>
        /// <typeparam name="TInterface">Api Interface.</typeparam>
        public ApiCaller<TInterface> Of<TInterface>()
            where TInterface : class
        {
            return new ApiCaller<TInterface>(client);
        }
    }
}
