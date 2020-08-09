using System;

namespace RpcLikeBlazor.Helpers
{
    internal static class ArgumentsHelpers
    {
        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if argument is <see langword="null"/>.
        /// </summary>
        /// <param name="arg">Argument.</param>
        /// <param name="argName">Argument name, may be <see langword="null"/>.</param>
        public static void ThrowIfNull(object arg, string argName = null)
        {
            if (arg is null)
            {
                if (argName is null)
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    throw new ArgumentNullException(argName);
                }
            }
        }
    }
}
