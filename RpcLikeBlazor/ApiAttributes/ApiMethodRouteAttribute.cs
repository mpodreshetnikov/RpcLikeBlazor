using System;

namespace RpcLikeBlazor.ApiAttributes
{
    /// <summary>
    /// Attribute for an Api Interface method describing method route.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApiMethodRouteAttribute : Attribute
    {
        /// <summary>
        /// Route for method (cannot be <see langword="null"/>).
        /// </summary>
        public string Route { get; }

        /// <summary>
        /// Constructor for <see cref="ApiMethodRouteAttribute"/>
        /// </summary>
        /// <param name="route">
        ///     <para>Route for method.</para>
        ///     <para>Default (if no <see cref="ApiMethodRouteAttribute"/> provided): Method name.</para>
        ///     <para>Default (if <paramref name="route"/> is <see langword="null"/> or whitespace): Empty route.</para>
        /// </param>
        public ApiMethodRouteAttribute(string route = null)
        {
            Route = route ?? "";
        }
    }
}
