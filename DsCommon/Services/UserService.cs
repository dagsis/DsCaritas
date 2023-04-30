using DsCommon.Models;
using DsCommon.ModelsView;

namespace DsCommon.Services
{
	public class UserService : BaseService, IUserService
    {
        private readonly IHttpClientFactory clienteFactory;

        public UserService(IHttpClientFactory clienteFactory) : base(clienteFactory)
        {
            this.clienteFactory = clienteFactory;
        }

        public async Task<T> LoginAsync<T>(LoginViewModel model)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.POST,
                Data = model,
                Url = SDRutas.IdentityApiBase + "/v1/Identity/login",               
            });            
        }

        public async Task<T> GetAllUserAsync<T>(string token, int companiaId, string userId)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.GET,
                Url = SDRutas.IdentityApiBase + "/v1/User/" + companiaId + "/" + userId,
                AccessToken = token
            });
        }

        public async Task<T> GetAllRolAsync<T>(string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.GET,
                Url = SDRutas.IdentityApiBase + "/v1/role",
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
                Url = SDRutas.IdentityApiBase + "/v1/rol",
                AccessToken = token
            });
        }

        public async Task<T> UpdateRol<T>(RoleViewModel model, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.PUT,
                Data = model,
                Url = SDRutas.IdentityApiBase + "/v1/rol",
                AccessToken = token
            });
        }

        public async Task<T> DeleteRolAsync<T>(string id, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.DELETE,
                Url = SDRutas.IdentityApiBase + "/v1/rol/" + id,
                AccessToken = token
            });
        }

        public async Task<T> GetUserByAsync<T>(string id, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.GET,
                Url = SDRutas.IdentityApiBase + "/v1/Identity/" + id,
                AccessToken = token
            });
        }

        public async Task<T> CreateUser<T>(UserViewModel model, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.POST,
                Data = model,
                Url = SDRutas.IdentityApiBase + "/v1/Identity/registerUser",
                AccessToken = token
            });
        }

        public async Task<T> UpdateUser<T>(UserViewModel model, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.PUT,
                Data = model,
                Url = SDRutas.IdentityApiBase + "/v1/Identity/updateUser",
                AccessToken = token
            });
        }

        public async Task<T> DeleteUserAsync<T>(string id, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.DELETE,
                Url = SDRutas.IdentityApiBase + "/v1/Identity/deleteUser/" + id,
                AccessToken = token
            });
        }

        public async Task<T> ChangePassword<T>(ChangePasswordViewModel model, string token, string userId)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.POST,
                Data = model,
                Url = SDRutas.IdentityApiBase + "/v1/Identity/updatepassword",
                AccessToken = token
            });

            //+ userId
        }


        public async Task<T> GeneratePasswordResetEmail<T>(ForgotPasswordViewModel model)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.POST,
                Data = model,
                Url = SDRutas.IdentityApiBase + "/v1/Identity/forgotpassword",
            });
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            return await this.SendAsync<string>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.GET,
                Url = SDRutas.IdentityApiBase + "/v1/Identity/GeneratePasswordResetToken/" + email,
            });
        }

        public async Task<T> GetUserByEmailAsync<T>(string email, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.GET,
                Url = SDRutas.IdentityApiBase + "/v1/Identity/email/" + email,
                AccessToken = token
            });
        }

        public async Task<string> EnviarEmail(EmailViewModel model)
        {
            return await this.SendAsync<string>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.POST,
                Data = model,
                Url = SDRutas.IdentityApiBase + "/v1/Identity/emailSender",
            });
        }

        public async Task<string> ResetPassword(ResetPasswordViewModel model)
        {
            return await this.SendAsync<string>(new ApiRequest()
            {
                ApiType = SDRutas.ApiType.POST,
                Data = model,
                Url = SDRutas.IdentityApiBase + "/v1/Identity/resetPassword",
            });
        }
    }
}
