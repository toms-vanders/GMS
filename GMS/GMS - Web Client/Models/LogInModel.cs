using System.ComponentModel.DataAnnotations;

namespace GMS___Web_Client.Models
{
    public class LogInModel
    {
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "You need to give us your email address to log in.")]
        public string EmailAddress { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "You must have a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}