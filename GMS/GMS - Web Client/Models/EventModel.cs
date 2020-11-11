using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GMS___Web_Client.Models
{
    public class EventModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Event name")]
        [Required(ErrorMessage = "Event name is required.")]
        public string EventName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Event type")]
        [Required(ErrorMessage = "Event type is required.")]
        public string EventType { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Event location")]
        [Required(ErrorMessage = "Event location is required.")]
        public string EventLocation { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Event date and time")]
        [Required(ErrorMessage = "Event date and time are required.")]
        public DateTime EventDateTime { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Event description")]
        [Required(ErrorMessage = "Event descriptipon is required.")]
        public string EventDescription { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Max. number of characters in event")]
        [Required(ErrorMessage = "Max. character number is required.")]
        public int EventMaxNumberOfCharacters { get; set; }

    }
}