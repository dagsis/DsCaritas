using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon
{
    public class SDRutas
    {
        public static string? EmailApiBase { get; set; }
        public static string? GateWayAPIBase { get; set; }
        public static string? IdentityApiBase { get; set; }
        public static int? CompaniaId { get; set; } = 0;
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
