using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace NSE.WebAPI.Core.Identity
{
    public class CustomAuthorization
    {
        public static bool ValidateUserClaims(HttpContext httpContext, string claimName, string claimValue)
        {
            return httpContext.User.Identity.IsAuthenticated &&
                    httpContext.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
        }
    }

    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequestClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) }; 
        }
    }

    public class RequestClaimFilter: IAuthorizationFilter
    {
        private readonly Claim _claim;

        public RequestClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext authorizationFilterContext)
        {
            if (!authorizationFilterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                authorizationFilterContext.Result = new StatusCodeResult(401);
                return;
            }

            if(!CustomAuthorization.ValidateUserClaims(authorizationFilterContext.HttpContext, _claim.Type, _claim.Value))
            {
                authorizationFilterContext.Result = new StatusCodeResult(403);
            }
        }
    }

}
