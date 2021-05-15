using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Result
{
    public class DataResult<T>:Result,IDataResult<T>
    {
        //burasi hem geriye bir data donderiyor hem mesaj verme imkani veriyor bize
        public DataResult(T data,bool success,string message):base(success,message)
        {
            Data = data;
        }
        public DataResult(T data,bool success):base(success)
        {
            Data = data;
        }
        public T Data { get; }
    }
}
