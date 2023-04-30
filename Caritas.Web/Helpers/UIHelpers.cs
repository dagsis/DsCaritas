using Caritas.Web.Extensions;

namespace Caritas.Web.Helpers
{
    public class UIHelpers
    {
        public static string GetRealName(IEnumerable<System.Security.Claims.Claim> claims)
        {
            string rtn = string.Empty;

            if (claims != null)
            {
                rtn = claims.ToList().GetClaimValue("name");
                if (string.IsNullOrEmpty(rtn))
                    rtn = claims.ToList().GetClaimValue("email");
            }

            return rtn;
        }
    }
}
