using RpcLikeBlazor.Helpers;
using System;
using System.Linq.Expressions;
using System.Net.Http;

namespace RpcLikeBlazor.Core.Middlewares
{
    /// <summary>
    /// Class that allow to add middlewares.
    /// </summary>
    public class Middlewares
    {
        /// <summary>
        /// Add on-request middleware. It will be executed when request just is created.
        /// If returns false then following middlewares will not be executed.
        /// </summary>
        public void OnRequest(Func<MethodCallExpression, bool> middleware)
        {
            ArgumentsHelpers.ThrowIfNull(middleware, nameof(middleware));
            MiddlewaresManager.OnRequestMiddlewares.Add(middleware);
        }

        /// <summary>
        /// Add on-request middleware. It will be executed when request just is created.
        /// </summary>
        public void OnRequest(Action<MethodCallExpression> middleware)
        {
            ArgumentsHelpers.ThrowIfNull(middleware, nameof(middleware));
            MiddlewaresManager.OnRequestMiddlewares.Add(methodCallExpression =>
            {
                middleware(methodCallExpression);
                return true;
            });
        }

        /// <summary>
        /// Add on-response middleware. It will be executed when response received.
        /// If returns false then following middlewares will not be executed.
        /// </summary>
        public void OnResponse(Func<HttpResponseMessage, bool> middleware)
        {
            ArgumentsHelpers.ThrowIfNull(middleware, nameof(middleware));
            MiddlewaresManager.OnResponseMiddlewares.Add(middleware);
        }

        /// <summary>
        /// Add on-response middleware. It will be executed when response received.
        /// </summary>
        public void OnResponse(Action<HttpResponseMessage> middleware)
        {
            ArgumentsHelpers.ThrowIfNull(middleware, nameof(middleware));
            MiddlewaresManager.OnResponseMiddlewares.Add(methodInfo =>
            {
                middleware(methodInfo);
                return true;
            });
        }

        /// <summary>
        /// Add on-exception middleware. It will be executed if exception will be thrown by request sending.
        /// If returns false then following middlewares will not be executed.
        /// </summary>
        public void OnResponseSendingException(Func<Exception, bool> middleware)
        {
            ArgumentsHelpers.ThrowIfNull(middleware, nameof(middleware));
            MiddlewaresManager.OnRequestSendingExceptionMiddlewares.Add(middleware);
        }

        /// <summary>
        /// Add on-exception middleware. It will be executed if exception will be thrown by request sending.
        /// </summary>
        public void OnResponseSendingException(Action<Exception> middleware)
        {
            ArgumentsHelpers.ThrowIfNull(middleware, nameof(middleware));
            MiddlewaresManager.OnRequestSendingExceptionMiddlewares.Add(exception =>
            {
                middleware(exception);
                return true;
            });
        }
    }
}
