using Lightstone.WebClient.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Lightstone.WebClient
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = identity.Ticket;

                        string ticketUserRole = ticket.UserData.Split(new string[] { "|" }, StringSplitOptions.None)[0];
                        string ticketDisplayName = ticket.UserData.Split(new string[] { "|" }, StringSplitOptions.None)[1];
                        string ticketTeamId = ticket.UserData.Split(new string[] { "|" }, StringSplitOptions.None)[2];
                        string ticketTeamName = ticket.UserData.Split(new string[] { "|" }, StringSplitOptions.None)[3];
                        
                        HttpContext.Current.User = new CustomPrincipal(identity, ticketUserRole, ticketDisplayName, Convert.ToInt32(ticketTeamId), ticketTeamName );
                    }
                }
            }
        }
    }
}
