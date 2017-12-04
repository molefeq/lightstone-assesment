using Lightstone.PresentationLayer.ViewModels;

using System;
using System.Web;
using System.Web.Security;

namespace Lightstone.WebClient.Authentication
{
    public class AuthenticationManager
    {
        private static AuthenticationManager _Instance;

        public static AuthenticationManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new AuthenticationManager();
                }

                return _Instance;
            }
        }

        private AuthenticationManager() { }

        public HttpCookie FormsAuthenticationLogin(EmployeeViewModel model)
        {
            string role = model.IsManager ? "Manager" : "Employee";

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.EmployeeId.ToString(), DateTime.Now, DateTime.Now.AddMinutes(60), false, role + "|" + model.FullName + "|" + model.TeamId.ToString() + "|" + model.TeamName,
                                                                             FormsAuthentication.FormsCookiePath);

            string encryptCookie = FormsAuthentication.Encrypt(ticket);
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptCookie);

            return authCookie;
        }

        public void FormsAuthenticationLogout(HttpSessionStateBase session, HttpResponseBase response)
        {
            FormsAuthentication.SignOut();
            session.Abandon();

            // clear authentication cookie
            HttpCookie formsAuthenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            formsAuthenticationCookie.Expires = DateTime.Now.AddYears(-1);
            response.Cookies.Add(formsAuthenticationCookie);

            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            response.Cookies.Add(cookie2);
        }
    }
}