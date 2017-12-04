using Lightstone.Common.CustomExceptions;

using Lightstone.DataAccess;
using Lightstone.DataAccess.Repository;

using Lightstone.PresentationLayer.Enums;
using Lightstone.PresentationLayer.ModelBuilders;
using Lightstone.PresentationLayer.ModelMutators;
using Lightstone.PresentationLayer.Models;
using Lightstone.PresentationLayer.ViewModelFilters;
using Lightstone.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lightstone.PresentationLayer.Services
{
    public class LeaveRequestService
    {
        private string _IncludeProperties = "ActionManager, Employee, RequestStatu";
        private static LeaveRequestService _Instance;

        public static LeaveRequestService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new LeaveRequestService();
                }

                return _Instance;
            }
        }

        private LeaveRequestService() { }

        public List<LeaveRequestViewModel> FetchLeaveRequests(LeaveRequestFilter filter)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                List<LeaveRequest> leaveRequests = unitOfWork.LeaveRequestRepository.GetLeaveRequests(filter.EmployeeId, filter.TeamId, filter.SearchText, null, null, _IncludeProperties);

                if (leaveRequests == null || leaveRequests.Count == 0)
                {
                    return new List<LeaveRequestViewModel>();
                }

                return leaveRequests.Select(l => LeaveRequestModelBuilder.Instance.MapToLeaveRequestViewModel(l)).ToList();
            }
        }

        public LeaveRequestModel FetchLeaveRequest(int leaveRequestId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                LeaveRequest leaveRequest = unitOfWork.LeaveRequestRepository.GetById(item => item.LeaveRequestId == leaveRequestId);

                if (leaveRequest == null)
                {
                    throw new LightstoneValidationException("Leave entry you trying to fetch does not exist.");
                }

                return  LeaveRequestModelBuilder.Instance.MapToLeaveRequestModel(leaveRequest);
            }
        }

        public void CreateLeaveRequest(LeaveRequestModel leaveRequestModel)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                CreateLeaveCheck(unitOfWork, leaveRequestModel);
                RequestStatu status = unitOfWork.RequestStatusRepository.GetRequestStatusByCode(Status.PENDING.ToString());

                leaveRequestModel.StatusId = status.RequestStatusId;
                unitOfWork.LeaveRequestRepository.Insert(LeaveRequestModelBuilder.Instance.MapToLeaveRequestDao(leaveRequestModel));
                unitOfWork.Save();
            }
        }

        public void UpdateLeaveRequest(LeaveRequestModel leaveRequestModel)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                UpdateLeaveCheck(unitOfWork, leaveRequestModel);

                LeaveRequest leaveRequest = unitOfWork.LeaveRequestRepository.GetById(item => item.LeaveRequestId == leaveRequestModel.LeaveRequestId);
                LeaveRequestMutator.Instance.MapToLeaveRequestDao(leaveRequest, leaveRequestModel);

                unitOfWork.LeaveRequestRepository.Update(leaveRequest);
                unitOfWork.Save();
            }
        }

        public void DeleteLeaveRequest(int leaveRequestId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                LeaveRequest leaveRequest = unitOfWork.LeaveRequestRepository.GetById(item => item.LeaveRequestId == leaveRequestId);

                if (leaveRequest == null)
                {
                    throw new LightstoneValidationException("Leave entry you trying to delete does not exist.");
                }

                unitOfWork.LeaveRequestRepository.Delete(leaveRequest);
                unitOfWork.Save();
            }
        }

        public void ApproveLeaveRequest(LeaveRequestModel leaveRequestModel)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                RequestStatu status = unitOfWork.RequestStatusRepository.GetRequestStatusByCode(Status.APPROVED.ToString());
                LeaveRequest leaveRequest = unitOfWork.LeaveRequestRepository.GetById(item => item.LeaveRequestId == leaveRequestModel.LeaveRequestId);

                if (leaveRequest == null)
                {
                    throw new LightstoneValidationException("Leave entry you trying to approve does not exist in the database.");
                }
                leaveRequestModel.LeaveRequestId = status.RequestStatusId;
                UpdateLeaveRequest(unitOfWork, leaveRequest, leaveRequestModel);
            }
        }

        public void DeclineLeaveRequest(LeaveRequestModel leaveRequestModel)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                RequestStatu status = unitOfWork.RequestStatusRepository.GetRequestStatusByCode(Status.APPROVED.ToString());
                LeaveRequest leaveRequest = unitOfWork.LeaveRequestRepository.GetById(item => item.LeaveRequestId == leaveRequestModel.LeaveRequestId);

                if (leaveRequest == null)
                {
                    throw new LightstoneValidationException("Leave entry you trying to decline does not exist in the database.");
                }

                leaveRequestModel.LeaveRequestId = status.RequestStatusId;
                UpdateLeaveRequest(unitOfWork, leaveRequest, leaveRequestModel);
            }
        }

        private void UpdateLeaveRequest(UnitOfWork unitOfWork, LeaveRequest leaveRequest, LeaveRequestModel leaveRequestModel)
        {
            leaveRequest.RequestStatusId = leaveRequestModel.LeaveRequestId.Value;
            leaveRequest.ActionManagerId = leaveRequestModel.ActionManagerId;

            unitOfWork.LeaveRequestRepository.Update(leaveRequest);
            unitOfWork.Save();
        }

        private void CreateLeaveCheck(UnitOfWork unitOfWork, LeaveRequestModel leaveRequestModel)
        {
            if (leaveRequestModel.LeaveFrom.Value.DayOfWeek == DayOfWeek.Saturday || leaveRequestModel.LeaveFrom.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new LightstoneValidationException("Leave cannot start on the weekend.");
            }

            if (leaveRequestModel.LeaveTo.Value.DayOfWeek == DayOfWeek.Saturday || leaveRequestModel.LeaveFrom.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new LightstoneValidationException("Leave cannot end on the weekend.");
            }

            if (leaveRequestModel.LeaveFrom.Value < DateTime.Now)
            {
                throw new LightstoneValidationException("Leave from must be in the future.");
            }

            if (leaveRequestModel.LeaveFrom.Value > leaveRequestModel.LeaveTo.Value)
            {
                throw new LightstoneValidationException("Leave from must be less than leave to.");
            }

            if (unitOfWork.LeaveRequestRepository.GetById(item => item.LeaveFrom <= leaveRequestModel.LeaveTo.Value &&
             item.LeaveTo >= leaveRequestModel.LeaveFrom.Value && item.EmployeeId == leaveRequestModel.EmployeeId.Value) != null)
            {
                throw new LightstoneValidationException("Leave entry you insert overlaps with an existing leave request.");
            }
        }

        private void UpdateLeaveCheck(UnitOfWork unitOfWork, LeaveRequestModel leaveRequestModel)
        {
            if (unitOfWork.LeaveRequestRepository.GetById(item => item.LeaveRequestId == leaveRequestModel.LeaveRequestId) == null)
            {
                throw new LightstoneValidationException("Leave entry you trying to delete does not exist.");
            }

            if (leaveRequestModel.LeaveFrom.Value.DayOfWeek == DayOfWeek.Saturday || leaveRequestModel.LeaveFrom.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new LightstoneValidationException("Leave cannot start on the weekend.");
            }

            if (leaveRequestModel.LeaveTo.Value.DayOfWeek == DayOfWeek.Saturday || leaveRequestModel.LeaveFrom.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new LightstoneValidationException("Leave cannot end on the weekend.");
            }

            if (leaveRequestModel.LeaveFrom.Value < DateTime.Now)
            {
                throw new LightstoneValidationException("Leave from must be in the future.");
            }

            if (leaveRequestModel.LeaveFrom.Value > leaveRequestModel.LeaveTo.Value)
            {
                throw new LightstoneValidationException("Leave from must be less than leave to.");
            }

            if (unitOfWork.LeaveRequestRepository.GetById(item => item.LeaveFrom <= leaveRequestModel.LeaveTo.Value &&
             item.LeaveTo >= leaveRequestModel.LeaveFrom.Value && item.LeaveRequestId != leaveRequestModel.LeaveRequestId.Value &&
             item.EmployeeId == leaveRequestModel.EmployeeId.Value) != null)
            {
                throw new LightstoneValidationException("Leave entry you insert overlaps with an existing leave request.");
            }
        }
    }
}
