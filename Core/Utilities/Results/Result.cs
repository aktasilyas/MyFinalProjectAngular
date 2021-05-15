using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Result
{
    public class Result : IResult
    {
        public Result(bool success,string message):this(success)
        {
            //burada eger iki parametreli halini kullanmak isterse asagida ki constructor da calisacak
            Message = message;
        }
        public Result(bool success)
        {
            Success = success;
        }
        // get readonly oldugu icin sadece constructor icinde set edilebilir
        public bool Success { get; }

        public string Message { get; }
    }
}
