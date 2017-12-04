using Lightstone.PresentationLayer.Models;

namespace Lightstone.PresentationLayer.ViewModels
{
    public class LeaveRequestViewModel : LeaveRequestModel
    {
        public string EmployeeName { get; set; }
        public string LeaveFromText { get; set; }
        public string LeaveToText { get; set; }
        public string ActionManagerName { get; set; }
        public string StatusName { get; set; }
        public string StatusCode { get; set; }
        public bool IsPending { get; set; }
    }
}