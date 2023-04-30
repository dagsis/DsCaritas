using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.Helpers
{
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public static class Roles
    {
        private static List<Role> _roles = new List<Role>();
        public static List<Role> TraerRoles()
        {
            _roles.Clear();
            _roles.Add(new Role()
            {
                Id = "Usuario",
                Name = "Usuario",
            });
            _roles.Add(new Role()
            {
                Id = "Empleado",
                Name = "Empleado",
            });
            return _roles;
        }
    }
}
