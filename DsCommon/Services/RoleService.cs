using DsCommon.Models;
using DsCommon.ModelsView;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.Services
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IHttpClientFactory _clienteFactory;
        public RoleService(IHttpClientFactory clienteFactory) : base(clienteFactory)
        {
            {
                _clienteFactory = clienteFactory;
            }
        }

        public async Task<T> GetAllRolAsync<T>(string token,int companiaId)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.GET,
                Url = SDRutas.IdentityApiBase + "/v1/role/" + companiaId,
                AccessToken = token
            });
        }

        public async Task<T> GetARolByAsync<T>(string id, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.GET,
                Url = SDRutas.IdentityApiBase + "/v1/rol/" + id,
                AccessToken = token
            });
        }

        public async Task<T> CreateRol<T>(RoleViewModel model, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.POST,
                Data = model,
                Url = SDRutas.IdentityApiBase + "/v1/role",
                AccessToken = token
            });
        }

        public async Task<T> UpdateRol<T>(RoleViewModel model, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.PUT,
                Data = model,
                Url = SDRutas.IdentityApiBase + "/v1/role",
                AccessToken = token
            });
        }

        public async Task<T> DeleteRolAsync<T>(string id, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.DELETE,
                Url = SDRutas.IdentityApiBase + "/v1/role/" + id,
                AccessToken = token
            });
        }

        public async Task<bool> CheckRoleAsync(string roleName,int companiaId, string token)
        {
            return await this.SendAsync<bool>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.GET,
                Url = SDRutas.IdentityApiBase + "/v1/role/isRoleExist/" + roleName + "/" + companiaId,
                AccessToken = token
            });
        }

        public async Task<T> GetModuleRoleAsync<T>(string token, int aplicacionId, string roleId)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.GET,
                Url = SDRutas.IdentityApiBase + "/v1/Role/roleModule/" + roleId + "/" + aplicacionId,
                AccessToken = token
            });
        }

        public async Task<T> SetModuleRoleAsync<T>(ValorRoleviewModel model,string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.POST,
                Data = model,
                Url = SDRutas.IdentityApiBase + "/v1/role/setModule",
                AccessToken = token
            });
        }
    }
}
