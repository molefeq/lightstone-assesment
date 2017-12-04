using Lightstone.PresentationLayer.Enums;
using Lightstone.PresentationLayer.Models;
using Lightstone.PresentationLayer.Services;
using Lightstone.PresentationLayer.ViewModelFilters;
using Lightstone.PresentationLayer.ViewModels;
using Lightstone.WebClient.Filters;

using System.Collections.Generic;
using System.Web.Mvc;

namespace Lightstone.WebClient.Controllers
{
    [LightstoneAuthorizationFilter()]
    public class LeaveRequestController : BaseController
    {
        [HttpGet]
        public ActionResult LeaveRequests(string searchText)
        {
            var filter = new LeaveRequestFilter
            {
                SearchText = searchText,
                TeamId = IsManager.Value ? TeamId : null,
                EmployeeId = IsManager.Value ? null : EmployeeId
            };

            List<LeaveRequestViewModel> requests = LeaveRequestService.Instance.FetchLeaveRequests(filter);

            return View(requests);
        }

        [HttpGet]
        [LightstoneAuthorizationFilter("Employee")]
        public ActionResult CreateLeaveRequest()
        {
            LeaveRequestModel leaveRequestModel = new LeaveRequestModel();

            ViewBag.Title = "Create Leave Request";

            return View("CreateLeaveRequest", leaveRequestModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [LightstoneAuthorizationFilter("Employee")]
        public ActionResult CreateLeaveRequest(LeaveRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.EmployeeId = EmployeeId;
            LeaveRequestService.Instance.CreateLeaveRequest(model);

            return RedirectToAction("LeaveRequests", "LeaveRequest");
        }

        [HttpGet]
        [LightstoneAuthorizationFilter("Employee")]
        public ActionResult UpdateLeaveRequest(int leaveReuestId)
        {
            LeaveRequestModel leaveRequestModel = LeaveRequestService.Instance.FetchLeaveRequest(leaveReuestId); 

            ViewBag.Title = "Edit Leave Request";

            return View("UpdateLeaveRequest", leaveRequestModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LightstoneAuthorizationFilter("Employee")]
        public ActionResult UpdateLeaveRequest(LeaveRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            LeaveRequestService.Instance.UpdateLeaveRequest(model);

            return RedirectToAction("LeaveRequests", "LeaveRequest");
        }


        [HttpPost]
        [LightstoneAuthorizationFilter("Employee")]
        public ActionResult DeleteLeaveRequest(LeaveRequestViewModel model)
        {
            LeaveRequestService.Instance.DeleteLeaveRequest(model.LeaveRequestId.Value);

            return Json(new { data_url = "LeaveRequest/LeaveRequests" });
        }

        [HttpPost]
        [LightstoneAuthorizationFilter("Manager")]
        public ActionResult ApproveLeaveRequest(LeaveRequestViewModel model)
        {
            model.ActionManagerId = EmployeeId;
            LeaveRequestService.Instance.ApproveLeaveRequest(model);

            return Json(new { data_url = "LeaveRequest/LeaveRequests" });
        }

        [HttpPost]
        [LightstoneAuthorizationFilter("Manager")]
        public ActionResult DeclineLeaveRequest(LeaveRequestViewModel model)
        {
            model.ActionManagerId = EmployeeId;
            LeaveRequestService.Instance.DeclineLeaveRequest(model);

            return Json(new { data_url = "LeaveRequest/LeaveRequests" });
        }
    }
}