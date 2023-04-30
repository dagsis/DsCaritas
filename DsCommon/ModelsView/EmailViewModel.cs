using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.ModelsView
{
    public class EmailViewModel
    {
        public string Servidor { get; set; }
        public string DisplayName { get; set; }
        public string Envia { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Asunto { get; set; }
        public string HtmlMessage { get; set; }
        public string Token { get; set; }
        public List<IFormFile> uploadfile { get; set; }

    }
}
