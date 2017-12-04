using System;
using System.ComponentModel.DataAnnotations;

namespace Lightstone.PresentationLayer.Models
{
    public class LeaveRequestModel
    {
        public int? LeaveRequestId { get; set; }
        public int? EmployeeId { get; set; }
        public int? ActionManagerId { get; set; }

        [Required(ErrorMessage = "From is required.")]
        [Display(Name = "From")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LeaveFrom { get; set; }

        [Required(ErrorMessage = "To is required.")]
        [Display(Name = "To")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LeaveTo { get; set; }

        [Display(Name = "Reason")]
        [StringLength(4000, ErrorMessage = "Reason cannot be more than 4000 characters.")]
        public string LeaveReason { get; set; }

        public int? StatusId { get; set; }
    }
}
