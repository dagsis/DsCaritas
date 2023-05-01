using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.ModelsView
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public string Name { get; set; }
        public int CompaniaId { get; set; }
        public int Estado { get; set; }
        public string Status { get; set; }
    }
}
