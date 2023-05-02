using DsCommon.Models;
using DsCommon.ModelsView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.Services
{
    public interface IRoleService : IBaseService
    {
        Task<T> GetAllRolAsync<T>(string token,int companiaId);
        Task<T> GetARolByAsync<T>(string id, string token);
        Task<T> CreateRol<T>(RoleViewModel model, string token);
        Task<T> UpdateRol<T>(RoleViewModel model, string token);
        Task<T> DeleteRolAsync<T>(string id, string token);
        Task<bool> CheckRoleAsync(string roleName,int companiaId,string token);
        Task<T> GetModuleRoleAsync<T>(string token, int aplicacionId, string RoleId);

    }
}
