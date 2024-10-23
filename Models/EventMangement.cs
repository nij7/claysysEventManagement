using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Linq;
using System.Web;

namespace EventManagement.Models
{
    public class Event
    {
        [Key]
        [DisplayName("Event id")]
        public int EventID { get; set; }
        [Required]
        [DisplayName("Event name")]
        public string EventName { get; set; }
        [Required]
        [DisplayName("Date of event")]
        [DataType(DataType.Date)]
        public string DateOfEvent { get; set; }
        [Required]
        [DisplayName("Descripton")]
        public string Description { get; set; }
        
        [Required]
        [DisplayName("Sub event 1")]
        public string Subevent1 { get; set; }
        [Required]
        [DisplayName("Sub description 1")]
        public string Subdesc1 { get; set; }
        [DisplayName("Sub event 2")]
        public string Subevent2 { get; set; }
        [DisplayName("Sub description 2")]
        public string Subdesc2 { get; set; }
        [DisplayName("Sub event 3")]
        public string Subevent3 { get; set; }
        [DisplayName("Sub description 3")]
        public string Subdesc3 { get; set; }
        [DisplayName("Sub event 4")]
        public string Subevent4 { get; set; }
        [DisplayName("Sub description 4")]
        public string Subdesc4 { get; set; }
        [DisplayName("Sub event 5")]
        public string Subevent5 { get; set; }
        [DisplayName("Sub description 5")]
        public string Subdesc5 { get; set; }


    }
}