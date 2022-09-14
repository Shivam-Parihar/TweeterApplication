using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetApp.Service.Response
{
    public class ServiceResponse<T>
    {

        public T Data { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = null;
      
    }
}
