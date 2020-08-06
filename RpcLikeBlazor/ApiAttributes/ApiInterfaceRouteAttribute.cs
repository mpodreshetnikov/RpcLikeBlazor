using System;

namespace RpcLikeBlazor.ApiAttributes
{
    /// <summary>
    /// Attribute for an api interface describing base controller route.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class ApiInterfaceRouteAttribute : Attribute
    {
        public string BaseRoute { get; }

        public ApiInterfaceRouteAttribute(string baseRoute)
        {
            BaseRoute = baseRoute ?? "";
        }
    }
}
