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
        /// Create <see cref="ApiCaller{TInterface}"/>.
        /// </summary>
        /// <typeparam name="TInterface">Api Interface.</typeparam>
        public ApiCaller<TInterface> Of<TInterface>()
            where TInterface : class
        {
            return new ApiCaller<TInterface>(client, objectConverter);
        }
    }
}
