using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RpcLikeBlazor.ApiServiceSetup.Abstractions;
using RpcLikeBlazor.ApiServiceSetup.Implementations.ObjectConverters;
using RpcLikeBlazor.Core.Middlewares;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace RpcLikeBlazor.ApiServiceSetup
{
    /// <summary>
    /// Setup manager for RpcLikeBlazor setup.
    /// </summary>
    internal static class SetupManager
    {
        public static void Setup(WebAssemblyHostBuilder builder, ApiServiceSettings settings)
        {
            builder.Services.TryAddSingleton<ApiService>();
            builder.Services.TryAddSingleton<IObjectConverter, DefaultObjectConverter>();

            SetupHttpClient(builder, settings);
            SetupApiInterfacesInstances(builder, settings);
            settings?.MiddlewaresSetup(new Middlewares());
        }

        private static void SetupHttpClient(WebAssemblyHostBuilder builder, ApiServiceSettings settings)
        {
            var httpClient = settings?.HttpClient
                ?? new HttpClient
                {
                    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
                };
            builder.Services.TryAddSingleton(httpClient);
        }

        private static void SetupApiInterfacesInstances(WebAssemblyHostBuilder builder, ApiServiceSettings settings)
        {
            if (settings?.ApiInterfacesSources == null)
            {
                return;
            }

            foreach (var apiInterface in settings.ApiInterfacesSources.SelectMany(ai => ai.ApiInterfaces).ToList())
            {
                if (!ApiConventionManager.CheckForConventions(apiInterface, settings))
                {
                    continue;
                }

                AddServiceForApiInterfaceType(apiInterface, builder);
            }
        }

        private static void AddServiceForApiInterfaceType(Type apiInterfaceType, WebAssemblyHostBuilder builder)
        {
            var type = typeof(ApiCaller<>).MakeGenericType(apiInterfaceType);
            var parameters = new[] { typeof(HttpClient), typeof(IObjectConverter) };

            var constructor = type.GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);

            builder.Services.TryAddSingleton(type, sp =>
                constructor.Invoke(
                    parameters
                    .Select(type => sp.GetRequiredService(type))
                    .ToArray()));
        }
    }
}
