using System.ComponentModel.DataAnnotations;

namespace AllupMVCProject.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
