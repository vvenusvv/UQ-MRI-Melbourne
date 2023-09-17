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
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Appointment Date")]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm:ss}")]
        [Display(Name = "Appointment Time")]
        public TimeSpan Time { get; set; }

        [Required]
        [Display(Name = "MRI Service Provider")]
        public int MriId { get; set; }
    }
}