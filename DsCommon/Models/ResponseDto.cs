using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.ModelsApi
{
    public class ResponseDto
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
    }
}
