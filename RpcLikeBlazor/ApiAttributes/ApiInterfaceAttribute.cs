using System;

namespace RpcLikeBlazor.ApiAttributes
{
    /// <summary>
    /// Attribute describing that this interface is an Api Interface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class ApiInterfaceAttribute : Attribute
    {
        /// <summary>
        /// Base route for methods in this Api Interface, may be <see langword="null"/>.
        /// </summary>
        public string BaseRoute { get; }

        /// <summary>
        /// Constructor for <see cref="ApiInterfaceAttribute"/>.
        /// </summary>
        /// <param name="baseRoute">
        ///     <para>Base route for methods in this Api Interface, may be <see langword="null"/>.</para>
        ///     <para>Default (if <see langword="null"/>): Api Interface name without 'I' in the start and 'Api' in the end if exists.</para>
        /// </param>
        public ApiInterfaceAttribute(string baseRoute = null)
        {
            BaseRoute = baseRoute;
        }
    }
}
