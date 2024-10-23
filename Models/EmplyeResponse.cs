using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagement.Models
{
    public class EmployeeResponse
    {
        [Key]
        public int ResponseID { get; set; }

        [ForeignKey("Event")]
        public int EventID { get; set; }
        public string EventName { get; set; }
       
        public Event Event { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public string Username { get; set; }
        public Signup User { get; set; }

        [Required]
        public DateTime ResponseDate { get; set; } = DateTime.Now;
        
        public string Status { get; set; }
    }
}