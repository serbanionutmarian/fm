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
            return new T()
            {
                Error = new ResponseError()
                {
                    Content = content
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
