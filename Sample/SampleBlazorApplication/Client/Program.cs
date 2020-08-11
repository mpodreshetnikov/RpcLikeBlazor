using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RpcLikeBlazor.ApiServiceSetup;
using RpcLikeBlazor.ApiServiceSetup.Abstractions;
using RpcLikeBlazor.ApiServiceSetup.Implementations.ApiInterfacesSources;
using RpcLikeBlazor.WebAssemblyExtensions;
using SampleBlazorApplication.Shared.ApiDeclarations;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SampleBlazorApplication.Client
{
    public static class Program
    {
        public static Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.UseRpcLikeBlazor(new ApiServiceSettings
            {
                HttpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5001/") },
                ApiInterfacesSources = new List<IApiInterfacesSource>
                {
                    new ApiInterfaceAttributeSource(typeof(ITodoApi).Assembly),
                },
            });

            return builder.Build().RunAsync();
        }
    }
}
