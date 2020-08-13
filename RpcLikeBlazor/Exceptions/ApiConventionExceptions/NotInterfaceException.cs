using System;
using System.Collections.Generic;

namespace RpcLikeBlazor.Exceptions.ApiConventionExceptions
{
    internal class NotInterfaceException : ApiConventionException
    {
        private const string ErrorMessage = "Api Interface must be interface.";

        public NotInterfaceException(Type issuedApiInterfaceType) : base(issuedApiInterfaceType)
        {
        }

        public IEnumerable<string> DuplicatedMethodsNames { get; set; }

        protected override string ApiConventionIssueMessage => ErrorMessage;
    }
}
