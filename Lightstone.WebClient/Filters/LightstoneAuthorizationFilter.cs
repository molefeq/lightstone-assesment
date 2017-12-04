using Lightstone.WebClient.Authentication;

using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Lightstone.WebClient.Filters
{
    public class LightstoneAuthorizationFilter : FilterAttribute, IAuthorizationFilter
    {
        private string[] _roles;

        public LightstoneAuthorizationFilter() { }

        public LightstoneAuthorizationFilter(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            CustomPrincipal user = filterContext.HttpContext.User as CustomPrincipal;
            bool isAuthorized = isUserAuthorized(user);
            bool isAuthenticated = user != null && filterContext.RequestContext.HttpContext.Request.IsAuthenticated;
            
            if (!isAuthenticated || !isAuthorized)
            {                
                filterContext.RequestContext.HttpContext.Session.Abandon();
                FormsAuthentication.SignOut();

                // clear authentication cookie
                HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                cookie1.Expires = DateTime.Now.AddYears(-1);
                filterContext.RequestContext.HttpContext.Response.Cookies.Add(cookie1);

                // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
                HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
                cookie2.Expires = DateTime.Now.AddYears(-1);
                filterContext.RequestContext.HttpContext.Response.Cookies.Add(cookie2);
                                                
                filterContext.Result = new RedirectToRouteResult("Default",
                    new System.Web.Routing.RouteValueDictionary{
                        {"controller", "Account"},
                        {"action", "Login"}
                    }, true);

                return;
            }
        }

        public bool isUserAuthorized(CustomPrincipal user)
        {
            if (user == null)
            {
                return false;
            }

            if (_roles == null)
            {
                return true;
            }

            if (string.IsNullOrEmpty(user.Role))
            {
                return false;
            }

            return _roles.Where(r => r.Equals(user.Role)).Count() > 0;
        }
    }
}

