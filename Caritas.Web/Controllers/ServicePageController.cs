using Caritas.Web.Models;
using DsCommon;
using DsCommon.IUnitOfWorkPatern;
using DsCommon.ModelsApi;
using DsCommon.ModelsView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Caritas.Web.Controllers
{
    public class ServicePageController : Controller
    {
        private readonly IUnitOfWork _unitWork;
        public ServicePageController(IUnitOfWork unitWork)
        {
            _unitWork = unitWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Lockscreen()
        {
            ViewBag.Reason = "lockscreen";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LockscreenCheck(string password)
        {
            var model = new LoginViewModel()
            {
                Email = ((ClaimsIdentity)User.Identity!).Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault(),
                Password = password,
                Remenberme = false,
                CompaniaId = Convert.ToInt32(SDRutas.CompaniaId)
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

                foreach (var claim in result.Claims!)
                {
                    var claim1 = new Claim(claim.Clave, claim.Valor);
                    claims.Add(claim1);
                }

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

                if (resultLogin.success)
                {
                    return LocalRedirectPermanent("/Home");
                }
            }

            return LocalRedirectPermanent("/ServicePage/Lockscreen");
        }
    }
}
