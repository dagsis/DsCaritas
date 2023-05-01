using DsCommon.ModelsView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.Services
{
    public interface IUserService : IBaseService
    {
        Task<T>  LoginAsync<T>(LoginViewModel model);
        Task<T> GetAllUserAsync<T>(string token, int companiaId, string UserId);
        Task<T> GetUserByAsync<T>(string id, string token);
        Task<T> GetUserByEmailAsync<T>(string email, string token);
        Task<T> CreateUser<T>(UserViewModel model, string token);
        Task<T> UpdateUser<T>(UserViewModel model, string token);
        Task<T> GeneratePasswordResetEmail<T>(ForgotPasswordViewModel model);
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<T> ChangePassword<T>(ChangePasswordViewModel model, string token, string userId);
        Task<T> DeleteUserAsync<T>(string id, string token);
        Task<string> EnviarEmail(EmailViewModel model);
        Task<string> ResetPassword(ResetPasswordViewModel model);
        Task<T> GetModuleUserAsync<T>(string token, int companiaId, string UserId);

    }
}
