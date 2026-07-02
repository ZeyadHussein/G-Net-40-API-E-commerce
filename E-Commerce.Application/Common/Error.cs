using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Common
{
    public sealed record Error(string code,string desciption,ErrorType type=ErrorType.Failure)
    {
        public static Error Failure(string code="General.Failure",string description="General Failure Has Occurred")
            => new Error(code,description,ErrorType.Failure);
        public static Error Validation(string code = "General.Validation", string description = "Validation Error Has Occurred")
            => new Error(code, description, ErrorType.Validation);
        public static Error NotFound(string code = "General.NotFound", string description = "The Requested Resoucrse Wasnot found")
            => new Error(code, description, ErrorType.NotFound);
        public static Error Conflict(string code = "General.Conflict", string description = "A Conflict Ocurred With The Current State")
            => new Error(code, description, ErrorType.Conflict);
        public static Error Unauthorized(string code = "General.Unauthorized", string description = "Acess Is Denied To Lack Authorized")
            => new Error(code, description, ErrorType.Unauthorized);
        public static Error Forbidden(string code = "General.Forbidden", string description = "the operation is forbidden")
            => new Error(code, description, ErrorType.Forbidden);
        public static Error InvalidCredentials(string code = "General.InvalidCredtials", string description = "the Provided Credentials Are Not Valid")
            => new Error(code, description, ErrorType.InvalidCredentials);

    }
}
