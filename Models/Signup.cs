using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EventManagement.Models
{
    public class Signup
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "First name contains only letters.")]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Last name contains only letters.")]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Date of birth")]
        [DataType(DataType.Date)]
        public string DateOfBirth { get; set; }


        [Required]
        [DisplayName("Gender")]

        public string Gender { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 numbers.")]
        [DisplayName("Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("E-mail")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DisplayName("Address")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }



        [Required]
        [DisplayName("State")]
        public string State { get; set; }

        [Required]
        [DisplayName("City")]
        public string City { get; set; }

        [Required]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@#$%^&+=!])[A-Za-z\d@#$%^&+=!]{8,}$",
        ErrorMessage = "Error!!.. example='Qwerty@24'")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords not matching")]
        public string ConfirmPassword { get; set; }
    }
}