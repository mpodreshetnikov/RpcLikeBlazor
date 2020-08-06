using System;

namespace RpcLikeBlazor.ApiAttributes
{
    /// <summary>
    /// Attribute for an api interface method describing method route.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ApiMethodRouteAttribute : Attribute
    {
        public string Route { get; }

        public ApiMethodRouteAttribute(string route)
        {
            Route = route ?? "";
        }
    }
}
