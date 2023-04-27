using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class ApiException : SystemException
    {
        public ApiException() { }
        public ApiException(int statusCode, string? message, string? details)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public string? Details { get; set; }
    }
}
