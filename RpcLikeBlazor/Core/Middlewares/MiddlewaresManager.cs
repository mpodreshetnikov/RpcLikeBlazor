using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;

namespace RpcLikeBlazor.Core.Middlewares
{
    internal static class MiddlewaresManager
    {
        public static IList<Func<MethodCallExpression, bool>> OnRequestMiddlewares { get; } = new List<Func<MethodCallExpression, bool>>();

        public static IList<Func<HttpResponseMessage, bool>> OnResponseMiddlewares { get; } = new List<Func<HttpResponseMessage, bool>>();

        public static IList<Func<Exception, bool>> OnRequestSendingExceptionMiddlewares { get; } = new List<Func<Exception, bool>>();

        /// <summary>
        /// Execute on-request middlewares pipeline.
        /// </summary>
        public static void Request(MethodCallExpression methodCallExpression)
        {
            foreach (var middleware in OnRequestMiddlewares)
            {
                if (!middleware(methodCallExpression))
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Execute on-response middlewares pipeline.
        /// </summary>
        public static void Response(HttpResponseMessage httpResponseMessage)
        {
            foreach (var middleware in OnResponseMiddlewares)
            {
                if (!middleware(httpResponseMessage))
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Execute on-exception middlewares pipeline.
        /// </summary>
        public static void ResponseSendingException(Exception exception)
        {
            foreach (var middleware in OnRequestSendingExceptionMiddlewares)
            {
                if (!middleware(exception))
                {
                    return;
                }
            }
            // If no one middleware return false.
            throw exception;
        }
    }
}
