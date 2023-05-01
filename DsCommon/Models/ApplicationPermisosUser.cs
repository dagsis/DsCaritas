using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.Models
{
    public class ApplicationPermisosUser
    {
        public int Id { get; set; }

        public int ApplicationModuloId { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string ApplicationUserId { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string R { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string W { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string U { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string D { get; set; }
    }
}
