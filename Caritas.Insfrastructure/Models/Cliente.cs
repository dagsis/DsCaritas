using Caritas.Insfrastructure.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caritas.Insfrastructure.Model
{
    public class Cliente : BaseDomainModel
    {

        [Column(TypeName = "varchar(80)")]
        public string? Apellido { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? Nombre { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? Domicilio { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? Localidad { get; set; }

        [Column(TypeName = "varchar(8)")]
        public string? CodigoPostal { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? Telefono { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? Documento { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string? Dest { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string? EstadoC { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? Email { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? Email2 { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? Celular { get; set; }

        [Column(TypeName = "varchar(255)")]

        public string CPagoElectronico { get; set; }
        public string? Observaciones { get; set; }
        public EstadoEnum Estado { get; set; }
        public string FullName => $"{Apellido} {Nombre}";


    }
}
