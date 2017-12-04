using Lightstone.Common.Utils;
using Lightstone.DataAccess;
using Lightstone.PresentationLayer.Enums;
using Lightstone.PresentationLayer.Models;
using Lightstone.PresentationLayer.ViewModels;
using System;

namespace Lightstone.PresentationLayer.ModelBuilders
{
    public class LeaveRequestModelBuilder
    {
        private static LeaveRequestModelBuilder _Instance;

        public static LeaveRequestModelBuilder Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new LeaveRequestModelBuilder();
                }

                return _Instance;
            }
        }

        private LeaveRequestModelBuilder() { }

        public LeaveRequest MapToLeaveRequestDao(LeaveRequestModel leaveRequestModel)
        {
            if (leaveRequestModel == null)
            {
                return null;
            }

            LeaveRequest leaveRequest = new LeaveRequest
            {
                EmployeeId = leaveRequestModel.EmployeeId.Value,
                LeaveFrom = leaveRequestModel.LeaveFrom.Value,
                LeaveTo = leaveRequestModel.LeaveTo.Value,
                LeaveDescription = leaveRequestModel.LeaveReason,
                RequestStatusId = leaveRequestModel.StatusId.Value
            };

            return leaveRequest;
        }

        public LeaveRequestModel MapToLeaveRequestModel(LeaveRequest leaveRequest)
        {
            if (leaveRequest == null)
            {
                return null;
            }

            LeaveRequestModel leaveRequestModel = new LeaveRequestModel
            {
                LeaveRequestId = leaveRequest.LeaveRequestId,
                EmployeeId = leaveRequest.EmployeeId,
                ActionManagerId = leaveRequest.ActionManagerId,
                LeaveFrom = leaveRequest.LeaveFrom,
                LeaveTo = leaveRequest.LeaveTo,
                LeaveReason = leaveRequest.LeaveDescription,
                StatusId = leaveRequest.RequestStatusId
            };

            return leaveRequestModel;
        }

        public LeaveRequestViewModel MapToLeaveRequestViewModel(LeaveRequest leaveRequest)
        {
            if (leaveRequest == null)
            {
                return null;
            }

            LeaveRequestViewModel leaveRequestViewModel = new LeaveRequestViewModel
            {
                LeaveRequestId = leaveRequest.LeaveRequestId,
                EmployeeId = leaveRequest.EmployeeId,
                ActionManagerId = leaveRequest.ActionManagerId,
                LeaveFrom = leaveRequest.LeaveFrom,
                LeaveFromText = leaveRequest.LeaveFrom.ToString(Constants.DisplayDateFormat),
                LeaveTo = leaveRequest.LeaveTo,
                LeaveToText = leaveRequest.LeaveTo.ToString(Constants.DisplayDateFormat),
                LeaveReason = leaveRequest.LeaveDescription,
                StatusId = leaveRequest.RequestStatusId,
                StatusCode = leaveRequest.RequestStatu.RequestStatusCode,
                StatusName = leaveRequest.RequestStatu.RequestStatusName
            };

            if (leaveRequest.Employee != null)
            {
                leaveRequestViewModel.EmployeeName = leaveRequest.Employee.FullName;
            }

            if (leaveRequest.ActionManager != null)
            {
                leaveRequestViewModel.ActionManagerName = leaveRequest.ActionManager.FullName;
            }

            Status status = (Status)Enum.Parse(typeof(Status), leaveRequestViewModel.StatusCode);

            switch (status)
            {
                case Status.APPROVED:
                    leaveRequestViewModel.StatusName = string.Format("Approved by: {0}", leaveRequestViewModel.ActionManagerName);
                    break;
                case Status.DECLINED:
                    leaveRequestViewModel.StatusName = string.Format("Declined by: {0}", leaveRequestViewModel.ActionManagerName);
                    break;
                case Status.PENDING:
                    leaveRequestViewModel.IsPending = true;
                    leaveRequestViewModel.StatusName = "Pending approval";
                    break;
            }

            return leaveRequestViewModel;
        }
    }
}
