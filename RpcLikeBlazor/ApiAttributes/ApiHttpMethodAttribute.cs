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
    /// Attribute for Api Interface method describing HTTP method.
    /// HttpGet method will be used if this attribute is not provided.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApiHttpMethodAttribute : Attribute
    {
        public HttpMethod HttpMethod { get; }

        public ApiHttpMethodAttribute(Method method)
        {
            HttpMethod = method switch
            {
                Method.Get => HttpMethod.Get,
                Method.Post => HttpMethod.Post,
                Method.Put => HttpMethod.Put,
                Method.Delete => HttpMethod.Delete,
                Method.Head => HttpMethod.Head,
                Method.Patch => HttpMethod.Patch,
                Method.Options => HttpMethod.Options,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
