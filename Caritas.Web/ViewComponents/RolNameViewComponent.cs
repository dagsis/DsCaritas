using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Caritas.Web.ViewComponents
{
    public class RolNameViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value).FirstOrDefault();

            return View("Default", roles);
        }
    }
}
