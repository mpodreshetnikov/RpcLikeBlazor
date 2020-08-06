using Newtonsoft.Json;
using RpcLikeBlazor.ApiAttributes;
using RpcLikeBlazor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace RpcLikeBlazor.Core
{
    /// <summary>
    /// Can call the api methods.
    /// </summary>
    /// <typeparam name="TInterface">Api interface.</typeparam>
    public class ApiCaller<TInterface>
            where TInterface : class
    {
        private const string SendAsyncMethodName = "SendAsync";
        private const string ContentPropertyName = "Content";
        private const string JsonContentMediaType = "application/json";

        private readonly HttpClient client;

        internal ApiCaller(HttpClient client)
        {
            this.client = client;
        }

        public Task Call(Expression<Action<TInterface>> methodCall)
        {
            return Call((MethodCallExpression)methodCall.Body);
        }

        public Task<TOut> Call<TOut>(Expression<Func<TInterface, Task<TOut>>> methodCall)
        {
            return Call<TOut>(methodCall.Body);
        }

        public Task<TOut> Call<TOut>(Expression<Func<TInterface, TOut>> methodCall)
        {
            return Call<TOut>(methodCall.Body);
        }

        private async Task<TOut> Call<TOut>(Expression methodExpression)
        {
            var response = await Call((MethodCallExpression)methodExpression).ConfigureAwait(false);
            var resultString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return CastReturnedValue<TOut>(resultString);
        }

        private TOut CastReturnedValue<TOut>(string resultString)
        {
            var returnType = typeof(TOut);

            if (resultString == null)
            {
                return default;
            }
            else if (returnType == typeof(DateTime))
            {
                object value = ParseHelpers.ParseDateTime(resultString);
                return (TOut)value;
            }
            else if (returnType.IsEnum)
            {
                var value = Enum.Parse(returnType, resultString);
                return (TOut)value;
            }
            else if (IsSimpleType(returnType))
            {
                return (TOut)Convert.ChangeType(resultString, returnType);
            }
            return JsonConvert.DeserializeObject<TOut>(resultString);
        }

        private async Task<HttpResponseMessage> Call(MethodCallExpression method)
        {
            // Get request method.
            var requestMethod = GetRequestMethodCallExpression(method);

            // Run request.
            var task = (Task<HttpResponseMessage>)Expression.Lambda(requestMethod).Compile().DynamicInvoke();
            var response = await task.ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            return response;
        }

        private MethodCallExpression GetRequestMethodCallExpression(MethodCallExpression methodCallExpression)
        {
            // Get values.
            var methodName = GetMethodName(methodCallExpression.Method);
            var methodArgsExpressions = methodCallExpression.Arguments.ToArray();
            var methodParametersNames = methodCallExpression.Method.GetParameters().Select(p => p.Name);

            var apiInterfaceMethodAttribute = methodCallExpression.Method.GetCustomAttribute<ApiInterfaceMethodAttribute>();
            var httpMethod = apiInterfaceMethodAttribute.Method;

            // Get method and parameters.
            var (apiMethod, parameters) = GetApiMethodAndParameters(
                methodName,
                httpMethod,
                methodParametersNames,
                methodArgsExpressions);

            // Call the method.
            var clientExpression = Expression.Constant(client);
            return Expression.Call(clientExpression, apiMethod, parameters);
        }

        private (MethodInfo, Expression[]) GetApiMethodAndParameters(
            string methodName,
            HttpMethod httpMethod,
            IEnumerable<string> argsNames,
            Expression[] argsValues)
        {
            // Setup query string for simple args and body for one complex arg.
            var queryDict = new Dictionary<string, string>();
            string bodyString = null;
            for (int i = 0; i < argsNames.Count(); i++)
            {
                var constValue = ReduceToConstant(argsValues[i]);
                var typeOfValue = constValue.GetType();
                if (IsSimpleType(typeOfValue))
                {
                    queryDict.Add(argsNames.ElementAt(i), constValue.ToString());
                }
                else if (string.IsNullOrEmpty(bodyString))
                {
                    bodyString = JsonConvert.SerializeObject(constValue);
                }
                else
                {
                    throw new ArgumentException("Only one argument can be complex type.");
                }
            }

            // Create url with query args.
            var url = methodName + queryDict.Aggregate("?", (s, kvp) => $"{s}{kvp.Key}={kvp.Value}&");

            // Construct HttpRequestMessage.
            var httpRequestMessageConstructor = typeof(HttpRequestMessage)
                .GetConstructor(new[] { typeof(HttpMethod), typeof(string) });
            var httpRequestMessageCtorExpression = Expression.New(
                httpRequestMessageConstructor,
                Expression.Constant(httpMethod),
                Expression.Constant(url));

            // Add prop initializer with Content initialize if needs.
            Expression httpRequestMessage = httpRequestMessageCtorExpression;
            if (httpMethod != HttpMethod.Get && httpMethod != HttpMethod.Head && bodyString != null)
            {
                httpRequestMessage = SetupContentPropertyInitializer(
                    httpRequestMessageCtorExpression,
                    GetBodyContentExpression(bodyString));
            }

            var parameters = new[] { httpRequestMessage };
            var apiMethod = typeof(HttpClient).GetMethod(SendAsyncMethodName, new[] { typeof(HttpRequestMessage) });
            return (apiMethod, parameters);
        }

        private bool IsSimpleType(Type type)
        {
            var asPrimitives = new[]
            {
                    typeof(DateTime),
                    typeof(Guid),
                    typeof(decimal),
                    typeof(string),
                };

            return type.IsPrimitive || type.IsEnum || asPrimitives.Contains(type);
        }

        private string GetMethodName(MethodInfo methodInfo)
        {
            var apiInterfaceRouteAttribute = typeof(TInterface).GetCustomAttribute<ApiInterfaceRouteAttribute>();
            var apiMethodRouteAttribute = methodInfo.GetCustomAttribute<ApiMethodRouteAttribute>();

            var route = apiInterfaceRouteAttribute?.BaseRoute ?? GetRouteByInterfaceName(typeof(TInterface).Name);
            if (apiMethodRouteAttribute != null)
            {
                route += $"/{apiMethodRouteAttribute.Route}";
            }
            else
            {
                route += $"/{methodInfo.Name}";
            }
            return route;
        }

        private string GetRouteByInterfaceName(string name)
        {
            if (name.StartsWith('I'))
            {
                name = name.Remove(0, 1);
            }
            if (name.EndsWith("Api"))
            {
                name = name[0..^3];
            }
            return name;
        }

        private object ReduceToConstant(Expression expression)
        {
            var objectMember = Expression.Convert(expression, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }

        private Expression GetBodyContentExpression(string bodyJsonContent)
        {
            var bodyContent = new StringContent(bodyJsonContent);
            bodyContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(JsonContentMediaType);
            return Expression.Constant(bodyContent);
        }

        private Expression SetupContentPropertyInitializer(NewExpression ctor, Expression value)
        {
            var property = typeof(HttpRequestMessage).GetProperty(ContentPropertyName);
            var binding = Expression.Bind(property, value);
            return Expression.MemberInit(ctor, binding);
        }
    }
}
