using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using Caritas.Web.Models;
using Caritas.Common;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using System.Text.Encodings.Web;
using DsCommon.IUnitOfWorkPatern;
using DsCommon.ModelsView;
using DsCommon;
using DsCommon.ModelsApi;

namespace Caritas.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUnitOfWork _unitWork;
        public AccountController(IWebHostEnvironment hostEnvironment, IUnitOfWork unitWork)
        {
            _hostEnvironment = hostEnvironment;
            _unitWork = unitWork;
        }
        public IActionResult Login()
        {
            TempData["page_function_js"] = "/js/functions/function_login.js";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(IFormCollection formdata, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("http://caritas.dagsist.net");

            LoginViewModel model = new LoginViewModel()
            {
                Email = formdata["Email"],
                Password = formdata["Password"],
                CompaniaId = (int)SDRutas.CompaniaId
            };


            using (var client = new HttpClient())
            {
                var resultLogin = await _unitWork.Usuarios.LoginAsync<ResponseDto>(model);
                if (!resultLogin.success)
                {
                    return Json(new { status = "error", msg = resultLogin.msg });
                }


                var result = JsonSerializer.Deserialize<IdentityAccess>(
                    resultLogin.data.ToString(),
                       new JsonSerializerOptions
                       {
                           PropertyNameCaseInsensitive = true
                       }
                    );

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result?.Id!),
                    new Claim(ClaimTypes.Name, result?.Nombre!),
                    new Claim(ClaimTypes.Email, result?.Email!),
                    new Claim(ClaimTypes.Role,result?.Roles?.FirstOrDefault()!),
                    new Claim("access_token", result?.Token!),
                    new Claim("compania", result?.CompaniaId.ToString()!)
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IssuedUtc = DateTime.UtcNow.AddHours(10)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Json(new { status = "success", msg = "Ok" });
            }
        
    }

        public IActionResult ForgotPassword()
        {
            var model = new ForgotPasswordViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var code = await _unitWork.Usuarios.GeneratePasswordResetTokenAsync(model.Email);

				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

				var callbackUrl = Url.Page(
					"/Account/ResetPassword",
					pageHandler: null,
					values: new {  Controller = "Account", Action = "ResetPassword", code },
					protocol: Request.Scheme);

				var PathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
						  + "templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
						  + Path.DirectorySeparatorChar.ToString() + "Forgot_Password.html";

				var subject = "Resetear Contraseña";
				string HtmlBody = "";
				using (StreamReader streamReader = System.IO.File.OpenText(PathToFile))
				{
					HtmlBody = streamReader.ReadToEnd();
				}

				////{0} : Subject  
				////{1} : DateTime  
				////{2} : Name  
				////{3} : Email  
				////{4} : Message  
				////{5} : callbackURL  

				string Message = $"Resetear su Password <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>click aqui</a>.";

				string messageBody = string.Format(HtmlBody,
					subject,
					String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
					"",
					model.Email,
					Message,
					callbackUrl
					);
				EmailViewModel emailViewModel = new EmailViewModel()
				{
					Asunto = subject,
					DisplayName = "Carlos D Agostino",
					Envia = model.Email,
					HtmlMessage = messageBody,
					Usuario = "dagsis@dagsis.com.ar",
					Password = "Q722rtg3",
                    Token = code
				};

			   await _unitWork.Usuarios.EnviarEmail(emailViewModel);

				return RedirectToAction("ForgotConfirmation");
            }

            return View(model); 
        }

		public IActionResult ForgotConfirmation()
		{
			return View();
		}

		public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }

		public IActionResult ResetPassword(string code)
		{
			TempData["page_function_js"] = "/js/functions/function_login.js";

			ResetPasswordViewModel model = new ResetPasswordViewModel()
			{
				Token = code
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
            if (ModelState.IsValid)
            {
				if (model.Token == null)
				{
					return BadRequest("A code must be supplied for password reset.");
				}

				string result = await _unitWork.Usuarios.ResetPassword(model);
				if (result == "Ok")
				{
					return Json(new { status = "success", msg = "Contraseña actualizada con éxito" });
				} else
                {
					return Json(new { status = "error", msg = result });
				}
                
			} else
            {
                return View(model);
            }
		}
	}
}
