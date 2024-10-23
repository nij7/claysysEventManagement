using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EventManagement.Models
{
    public class ContactUs
    {
        public int ContactId { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        [DisplayName("Name")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
    }
}