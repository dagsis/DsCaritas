using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.ModelsTable
{
    public class TableUIExt
    {

        [JsonProperty("apiUrl")]
        public string? ApiUrl { get; set; }

        [JsonProperty("displayLength")]
        public int DisplayLength { get; set; } = 10;

        [JsonProperty("viewOrder")]
        public OrderUI[]? ViewOrder { get; set; }

        [JsonProperty("dom")]
        public string Dom { get; set; } = "lBfrtip";

        [JsonProperty("widthAcciones")]
        public string WidthAcciones { get; set; } = "10%";

        [JsonProperty("createUrl")]
        public string? CreateUrl { get; set; }

        [JsonProperty("deleteUrl")]
        public string? DeleteUrl { get; set; }

        [JsonProperty("isOnModalCreate")]
        public bool IsOnModalCreate { get; set; } = true;

        [JsonProperty("metodo")]
        public string Metodo { get; set; } = "GET";

        [JsonProperty("isServerSide")]
        public string IsServerSide { get; set; } = "false";


        [JsonProperty("fields")]
        public FieldUI[]? Fields { get; set; }

        [JsonProperty("methods")]
        public MethodsUI[]? Methods { get; set; }

        [JsonProperty("isCreateAllow")]
        public bool IsCreateAllow { get; set; } = true;

        [JsonProperty("isPermisoCreate")]
        public bool IsOnPermisoCreate { get; set; } = true;
      
        public string FieldString
        {
            get
            {
                string rtn = "";

                foreach (FieldUI f in Fields!)
                {
                    rtn += (!string.IsNullOrEmpty(rtn) ? ", " : "") + $"{{ data: '{f.Data}'";
                    rtn += ", visible: " + f.Visible.ToString().ToLower();
                    if (!string.IsNullOrEmpty(f.Render))
                        rtn += ", render: " + f.Render;
                    rtn += " }";
                }

                return "[" + rtn + "]";
            }
        }


        public string MethodString
        {
            get
            {
                string rtn = "";

                foreach (MethodsUI f in Methods!)
                {
                    rtn += (!string.IsNullOrEmpty(rtn) ? ", " : "") + $"{{ url: '{f.Url}'";
                    rtn += ", titulo: " + f.titulo;
                    rtn += ", icono: " + f.Icono;
                    rtn += ", isModal: " + f.IsOnModal.ToString().ToLower();
                    rtn += " }";
                }
                return "[" + rtn + "]";
            }
        }

        public string OrdenString
        {
            get
            {
                string rtn = "";

                foreach (OrderUI f in ViewOrder)
                {
                    rtn += ", Orden: " + f.Order;
                    rtn += " }";
                }
                return "[" + rtn + "]";
            }
        }

      
    }
}
