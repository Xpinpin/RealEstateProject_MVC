using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSWProject.Models
{
    public class ListingContract
    {

        //[Key]
        //[Column(Order = 1)]
        //public int AgentId { get; set; }

        //[Key]
        //[Column(Order = 2)]
        //public int ListingId { get; set; }

        [Key]
        public int ContractID { get; set; }

        //[Required]
        //[Display(Name = "Sales Price")]
        //[Range(0, double.MaxValue, ErrorMessage = "You must enter a valid number for sales price")]
        //public double SalesPrice { get; set; }

        [Display(Name = "Contract's Start Date")]
        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Contract's End Date")]
        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Customer's Signature")]
        [CustomerSigned(Value = true, ErrorMessage = "The Customer must sign to continue.")]
        public bool IsSigned { get; set; }

        public int AgentId { get; set; }

        public virtual Agent Agent { get; set; }

        public int ListingId { get; set; }
        public virtual Listing Listing { get; set; }
    }
}