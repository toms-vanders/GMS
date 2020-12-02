using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GMS___Web_Client.Models
{
    public class LogInModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        [Required(ErrorMessage = "You need to give us your username to log in.")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "You must have a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}