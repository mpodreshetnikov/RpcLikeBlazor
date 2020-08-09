using RpcLikeBlazor.Helpers;
using System;
using System.Text;

namespace RpcLikeBlazor.Exceptions.ApiConventionExceptions
{
    internal class ApiConventionException : Exception
    {
        private const string BaseForIssueMessage = "Issue with api convention for ";

        protected virtual string ApiConventionIssueMessage { get; }

        public Type IssuedApiInterfaceType { get; }

        public ApiConventionException(Type issuedApiInterfaceType)
        {
            ArgumentsHelpers.ThrowIfNull(issuedApiInterfaceType, nameof(issuedApiInterfaceType));
            IssuedApiInterfaceType = issuedApiInterfaceType;
        }

        public override string Message => new StringBuilder(BaseForIssueMessage)
            .AppendLine(IssuedApiInterfaceType.FullName)
            .Append(ApiConventionIssueMessage)
            .ToString();
    }
}
