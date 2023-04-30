using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.ModelsTable
{
    public class OrderUI
    {
        [JsonProperty("order")]
        public int Order { get; set; } = 0;
        public string Direccion { get; set; } = "asc";
    }
}
