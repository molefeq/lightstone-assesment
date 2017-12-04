using Lightstone.Common.CustomExceptions;
using Lightstone.PresentationLayer.Services;
using Lightstone.PresentationLayer.ViewModels;
using Lightstone.WebClient.Authentication;
using Lightstone.WebClient.Filters;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Lightstone.WebClient.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            EmployeeViewModel response = EmployeeService.Instance.Login(model.EmailAddress);

            if (response == null)
            {
                throw new LightstoneValidationException("There is no user with a corresponding email address.", "EmailAddress" );
            }

            Response.Cookies.Add(AuthenticationManager.Instance.FormsAuthenticationLogin(response));

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("LeaveRequests", "LeaveRequest");
        }

        [HttpGet]
        [LightstoneAuthorizationFilter()]
        public ActionResult Logout()
        {
            AuthenticationManager.Instance.FormsAuthenticationLogout(Session, Response);
            return RedirectToAction("Login", "Account");
        }
    }
}