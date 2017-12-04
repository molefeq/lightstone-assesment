using System.ComponentModel.DataAnnotations;

namespace Lightstone.PresentationLayer.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email Address.")]
        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string EmailAddress { get; set; }
    }
}