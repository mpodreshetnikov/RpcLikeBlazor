using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RpcLikeBlazor.Exceptions.ApiConventionExceptions
{
    internal class FewComplexParametersException : ApiConventionException
    {
        private const string ErrorMessage = "Only one complex parameter supported in Api Interface method.";

        public FewComplexParametersException(Type issuedApiInterfaceType) : base(issuedApiInterfaceType)
        {
        }

        public IEnumerable<ParameterInfo> ComplexParameters { get; set; }

        public MethodInfo ApiInterfaceMethod { get; set; }

        protected override string ApiConventionIssueMessage => new StringBuilder(ErrorMessage)
            .AppendLine()
            .Append(ApiInterfaceMethod?.DeclaringType?.FullName).Append('.').AppendLine(ApiInterfaceMethod?.Name)
            .Append("Parameters: ").AppendJoin(", ", ComplexParameters.Select(cp => cp.Name))
            .ToString();
    }
}
