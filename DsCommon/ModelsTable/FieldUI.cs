using Newtonsoft.Json;

namespace DsCommon.ModelsTable
{
    public class FieldUI
    {
        [JsonProperty("data")]
        public string Data { get; set; }


        [JsonProperty("label")]
        public string Label { get; set; }


        [JsonProperty("render")]
        public string Render { get; set; }


        [JsonProperty("visible")]
        public bool Visible { get; set; } = true;

        [JsonProperty("columWidth")]
        public string ColumWidth { get; set; } = "10%";

        [JsonProperty("tipo")]
        public string Tipo { get; set; } = "S";

        [JsonProperty("columnaNumber")]
        public int ColumnaNumber { get; set; }

        [JsonProperty("clase")]
        public string Clase { get; set; }
       
    }
}
