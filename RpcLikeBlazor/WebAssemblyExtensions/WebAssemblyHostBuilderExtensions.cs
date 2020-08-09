using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RpcLikeBlazor.ApiServiceSetup;

namespace RpcLikeBlazor.WebAssemblyExtensions
{
    public static class WebAssemblyHostBuilderExtensions
    {
        public static WebAssemblyHostBuilder UseRpcLikeBlazor(
            this WebAssemblyHostBuilder builder,
            ApiServiceSettings settings = null)
        {
            SetupManager.Setup(builder, settings);
            return builder;
        }
    }
}
