using Lightstone.DataAccess;
using Lightstone.PresentationLayer.Models;

namespace Lightstone.PresentationLayer.ModelMutators
{
    public class LeaveRequestMutator
    {
        private static LeaveRequestMutator _Instance;

        public static LeaveRequestMutator Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new LeaveRequestMutator();
                }

                return _Instance;
            }
        }

        private LeaveRequestMutator() { }

        public void MapToLeaveRequestDao(LeaveRequest leaveRequest, LeaveRequestModel leaveRequestModel)
        {
            if (leaveRequestModel == null)
            {
                return;
            }

            leaveRequest.LeaveFrom = leaveRequestModel.LeaveFrom.Value;
            leaveRequest.LeaveTo = leaveRequestModel.LeaveTo.Value;
            leaveRequest.LeaveDescription = leaveRequestModel.LeaveReason;
        }
    }
}
