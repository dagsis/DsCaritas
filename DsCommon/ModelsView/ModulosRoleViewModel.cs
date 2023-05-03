using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.ModelsView
{
    public class ModulosRoleViewModel
    {
        public int Codigo { get; set; }
        public int AplicacionId { get; set; }
        public string Titulo { get; set; }
        public PermisosRoleViewModel Permisos { get; set; }
    }

    public class PermisosRoleViewModel
    {
        public int ApplicationModuloRoleId { get; set; }
        public string ApplicationRoleId { get; set; }
        public string R { get; set; }
        public string W { get; set; }
        public string U { get; set; }
        public string D { get; set; }
    }

    public class ValorRoleviewModel
    {
        public string RoleId { get; set; }
        public string Values { get; set; }
    }
}
