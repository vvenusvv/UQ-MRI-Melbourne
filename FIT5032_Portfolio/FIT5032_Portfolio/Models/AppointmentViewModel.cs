using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FIT5032_Portfolio.Models
{
    public class MakeAppointmentModel
    {
        [Required]
        [Display(Name = "Appointment Date")]
        public System.DateTime Date{ get; set; }

        [Required]
        [Display(Name = "Appointment Time")]
        public System.TimeSpan Time { get; set; }

        [Required]
        [Display(Name = "MRI Service Provider")]
        public int MriId { get; set; }
    }
}