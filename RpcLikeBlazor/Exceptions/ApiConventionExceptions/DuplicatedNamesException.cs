using System;
using System.Collections.Generic;
using System.Text;

namespace RpcLikeBlazor.Exceptions.ApiConventionExceptions
{
    internal class DuplicatedNamesException : ApiConventionException
    {
        private const string ErrorMessage = "Only one method name with certain Http Method supported in Api Interface.";

        public DuplicatedNamesException(Type issuedApiInterfaceType) : base(issuedApiInterfaceType)
        {
        }

        public IEnumerable<string> DuplicatedMethodsNames { get; set; }

        protected override string ApiConventionIssueMessage => new StringBuilder(ErrorMessage)
            .AppendLine()
            .Append("Duplicated methods names: ").AppendJoin(", ", DuplicatedMethodsNames)
            .ToString();
    }
}
