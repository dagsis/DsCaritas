using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.ModelsView
{
    public class ModulosRoleViewModel
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public int AplicacionId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public PermisosRoleViewModel Permisos { get; set; }
    }

    public class PermisosRoleViewModel
    {
        public int Id { get; set; }
        public int ApplicationModuloUserId { get; set; }
        public string ApplicationUserId { get; set; }
        public string R { get; set; }
        public string W { get; set; }
        public string U { get; set; }
        public string D { get; set; }
    }
}
