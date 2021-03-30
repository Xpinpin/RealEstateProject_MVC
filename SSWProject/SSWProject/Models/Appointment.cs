using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SSWProject.Models
{
    public class Appointment
    {
        public int ID {get; set; }

        [Required]
        [Display(Name = "Agent")]

        public int AgentID { get; set; }
        public virtual Agent Agents { get; set; }

        [Required]      
        [Display(Name = "Customer")]
        
        public int CustomerID { get; set; }
        public virtual Customer Customers { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]


        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Start time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = false)]


        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = false)]


        public DateTime EndTime { get; set; }

        public string Comment { get; set; }
        [NotMapped]
        public IEnumerable<Customer> GetCustomersList { get; set; }
        
        [NotMapped]
        public IEnumerable<Agent> GetAgentsList { get; set; }
    }
}