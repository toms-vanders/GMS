using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GMS___Web_Client.Models
{
    public class ApiModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Api Key")]
        [Required(ErrorMessage = "You need to give us your api key to enjoy all the awesome stuff that our program has to offer.")]
        public string ApiKey { get; set; }
    }
}