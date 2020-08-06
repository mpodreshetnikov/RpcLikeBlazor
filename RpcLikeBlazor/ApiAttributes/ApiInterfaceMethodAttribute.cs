using System;
using System.Net.Http;

namespace RpcLikeBlazor.ApiAttributes
{
    /// <summary>
    /// HTTP methods.
    /// </summary>
    public enum Method
    {
        Get,
        Post,
        Put,
        Delete,
        Head,
        Options,
        Patch
    }

    /// <summary>
    /// Attribute for api interface method describing HTTP method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ApiInterfaceMethodAttribute : Attribute
    {
        public HttpMethod Method { get; }

        public ApiInterfaceMethodAttribute(Method method)
        {
            Method = method switch
            {
                ApiAttributes.Method.Get => HttpMethod.Get,
                ApiAttributes.Method.Post => HttpMethod.Post,
                ApiAttributes.Method.Put => HttpMethod.Put,
                ApiAttributes.Method.Delete => HttpMethod.Delete,
                ApiAttributes.Method.Head => HttpMethod.Head,
                ApiAttributes.Method.Patch => HttpMethod.Patch,
                ApiAttributes.Method.Options => HttpMethod.Options,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
