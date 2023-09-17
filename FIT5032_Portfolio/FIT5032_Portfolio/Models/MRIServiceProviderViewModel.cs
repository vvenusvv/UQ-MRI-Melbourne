using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FIT5032_Portfolio.Models
{
    public class AddMRIServiceProviderModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Postcode")]
        public string Postcode { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm:ss}")]
        [Display(Name = "Opening Time")]
        public TimeSpan Open { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm:ss}")]
        public TimeSpan Close { get; set; }

        [Display(Name = "Latitude")]
        public string Lat { get; set; }

        [Display(Name = "Longitude")]
        public string Long { get; set; }

    }
}