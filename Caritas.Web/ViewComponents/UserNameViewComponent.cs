using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Caritas.Web.ViewComponents
{
    public class UserNameViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var nombre = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).FirstOrDefault();

            return View("Default", nombre);
        }
    }
}
