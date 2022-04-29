using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiDomotica.Core.Api
{
    public class ResponseResult
    {
        public bool Error { get; set; }
        public string ErrorTitle { get; set; }
        public string ErrorMsg { get; set; }
        public object ObjectInAction { get; set; }
        public object Context { get; set; }

        public int StatusCode { get; set; } = 200;
        public object Result { get; set; }
        public string ResultType { get; set; }
    }
}
