using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DsCommon.Enums;

namespace DsCommon.Models
{
    public class ApplicationModuloUser
    {
        public int Id { get; set; }
        public int CompaniaId { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "Descripción")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Column(TypeName = "varchar(80)")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public EstadoEnum Status { get; set; }

        public ApplicationPermisosUser Permisos { get; set; }
    }
}
