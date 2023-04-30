using System.Security.Claims;

namespace Caritas.Web.Extensions
{
    public static class ClaimExtensions
    {
        public static Claim GetClaim(this IEnumerable<Claim> claims, string claimName)
        {
            Claim rtn = null;

            if (!string.IsNullOrEmpty(claimName))
            {
                rtn = claims.FirstOrDefault(x => x.Type == claimName);
            }

            return rtn;
        }

        /// <summary>
        /// Gets the value of the requested claim if it exists
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="claimName"></param>
        /// <returns></returns>
        public static string GetClaimValue(this IEnumerable<Claim> claims, string claimName)
        {
            string rtn = null;

            if (!string.IsNullOrEmpty(claimName))
            {
                Claim claim = GetClaim(claims, claimName);
                if (claim != null)
                    rtn = claim.Value;
            }

            return rtn;
        }

        /// <summary>
        /// Updates a claim with a new value
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="claimName"></param>
        /// <param name="newValue"></param>
        public static void UpdateClaim(this List<Claim> claims, string claimName, string newValue)
        {
            Claim claim = claims.GetClaim(claimName);
            if (claim != null)
                claims.Remove(claim);

            claims.Add(new Claim(claimName, newValue));
        }
    }
}
