using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.ModelsTable
{
    public class MethodsUI
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("titulo")]
        public string titulo { get; set; }

        [JsonProperty("icono")]
        public string Icono { get; set; }

        [JsonProperty("isOnModal")]
        public bool IsOnModal { get; set; } = true;

        [JsonProperty("clase")]
        public string Clase { get; set; } = "btn btn-info btn-sm";

    }
}
