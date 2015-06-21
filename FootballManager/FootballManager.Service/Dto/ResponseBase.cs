using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dto
{
    public class ResponseBase
    {
        public ResponseError Error { get; set; }

        public static T CreateValidationError<T>(string content)
             where T : ResponseBase, new()
        {
            return CreateError<T>(content, true);
        }
        public static T CreateUnexpectedError<T>()
             where T : ResponseBase, new()
        {
            return CreateError<T>("Unexpected error", false);
        }
        private static T CreateError<T>(string content, bool isValidationError)
            where T : ResponseBase, new()
        {
            return new T()
            {
                Error = new ResponseError()
                {
                    Content = content,
                    IsValidationError = isValidationError
                }
            };
        }
    }
    public class ResponseError
    {
        public bool IsValidationError { get; set; }

        public string Content { get; set; }

        public bool AuthenticatedError { get; set; }
    }
}
