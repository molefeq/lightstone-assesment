using Lightstone.Common.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lightstone.WebClient.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception == null)
            {
                return;
            }


            if (filterContext.Exception is LightstoneValidationException)
            {
                LightstoneValidationException lightstoneValidationException = filterContext.Exception as LightstoneValidationException;
                HandleValidationException(filterContext.Exception as LightstoneValidationException, filterContext);
                return;
            }

            ViewResult result = new ViewResult()
            {
                ViewName = "Error"
            };


            filterContext.ExceptionHandled = true;
            result.ViewBag.ErrorMessage = filterContext.Exception.Message;
            filterContext.Result = result;
        }

        private void HandleValidationException(LightstoneValidationException lightstoneValidationException, ExceptionContext filterContext)
        {
            if (lightstoneValidationException == null)
            {
                return;
            }

            ViewResult result = new ViewResult()
            {
                ViewName = filterContext.RouteData.GetRequiredString("action")
            };

            filterContext.ExceptionHandled = true;

            if (lightstoneValidationException.Messages != null && lightstoneValidationException.Messages.Count > 0)
            {
                lightstoneValidationException.Messages.ForEach(item => result.ViewData.ModelState.AddModelError("", item));
            }

            if (!string.IsNullOrEmpty(lightstoneValidationException.FieldName))
            {
                result.ViewData.ModelState.AddModelError(lightstoneValidationException.FieldName, lightstoneValidationException.Message);
            }

            filterContext.Result = result;
        }
    }
}
