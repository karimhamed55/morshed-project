using System.ComponentModel.DataAnnotations;

namespace Morshed.Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}