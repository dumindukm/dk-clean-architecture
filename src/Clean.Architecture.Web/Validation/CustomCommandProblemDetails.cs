using Clean.Architecture.Application.Projects.Commands.CreateProject;
using Clean.Architecture.Application.Validation;
using Clean.Architecture.Core.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clean.Architecture.Web.Validation
{
    public class InvalidCommandProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public InvalidCommandProblemDetails(InvalidCommandException exception)
        {
            this.Title = exception.Message;
            this.Status = StatusCodes.Status400BadRequest;
            this.Detail = exception.Details;
            this.Type = "https://somedomain/validation-error";
        }
    }

    public class DomainModelValidationExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public DomainModelValidationExceptionProblemDetails(DomainModelValidationException exception)
        {
            this.Title = exception.BaseException.Message;
            this.Status = StatusCodes.Status400BadRequest;
            //this.Detail = exception.Details;
            this.Type = "https://somedomain/business-rule-validation-error";
        }
    }

    public class BusinessRuleValidationExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public BusinessRuleValidationExceptionProblemDetails(BusinessRuleValidationException exception)
        {
            this.Title = exception.Message;
            this.Status = StatusCodes.Status409Conflict;
            this.Detail = exception.Details;
            this.Type = "https://somedomain/business-rule-validation-error";
        }
    }


}
