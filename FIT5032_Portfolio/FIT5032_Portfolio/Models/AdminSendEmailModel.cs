using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FIT5032_Portfolio.Models
{
    public class AdminSendEmailModel
    {
        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Contents")]
        public string Contents { get; set; }

        [Required]
        [Display(Name = "Attachment (if any)")]
        public string Attachment { get; set; }
    }
}