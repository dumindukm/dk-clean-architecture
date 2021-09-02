using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Validation
{
    public class DomainModelValidationException : Exception 
    {
        public Exception BaseException { get; }
        public DomainModelValidationException(string message, Exception innerException) : base(message, innerException)
        {
            this.BaseException = innerException;
        }

    }

    public class BusinessRuleValidationException : Exception
    {
        public string Details { get; }
        public BusinessRuleValidationException(string message, string details) : base(message)
        {
            this.Details = details;
        }
    }
}
