using Lightstone.WebClient.Authentication;
using System;
using System.Web.Mvc;

namespace Lightstone.WebClient.Controllers
{
    public class BaseController : Controller
    {
        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }
        
        protected virtual int? EmployeeId
        {
            get
            {
                if (User == null)
                {
                    return null;
                }

                return Convert.ToInt32(User.Identity.Name);
            }
        }

        protected virtual bool? IsManager
        {
            get
            {
                if (User == null)
                {
                    return null;
                }

                return User.IsInRole("Manager");
            }
        }

        protected virtual int? TeamId
        {
            get
            {
                if (User == null)
                {
                    return null;
                }

                return User.TeamId;
            }
        }
    }
}