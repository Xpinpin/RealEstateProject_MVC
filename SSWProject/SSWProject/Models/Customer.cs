using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SSWProject.Models
{
    public class Customer
    {
        [Required]
        public int ID { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [StringLength(30, ErrorMessage = "The first name can not exceed 30 characters")]
        public string FirstName { get; set; }


        [Display(Name = "Middle Name")]
        [StringLength(10, ErrorMessage = "The middle initial can not exceed 10 characters")]
        public string MiddleName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        [StringLength(40, ErrorMessage = "The last name can not exceed 40 characters")]
        public string LastName { get; set; }

        [NotMapped]
        [Display(Name = "Customer Name")]
        public string CustomerName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Required]
        [Display(Name = "Street Name")]
        [StringLength(150, ErrorMessage = "Street Address must be in length of between 2 and 150 characters")]
        public string StreetAddress { get; set; }


        [Required]
        [Display(Name = "City")]
        [StringLength(50, ErrorMessage = "City must be in length of between 2 and 25 characters")]
        public string Municipality { get; set; }


        [Required]
        public Province? Province { get; set; }


        [Required]
        [Display(Name = "Postal Code")]
        [DataType(DataType.PostalCode)]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$", ErrorMessage = "Postal Code must be in correct format(Ex:E1G 2H5)")]
        [StringLength(7, ErrorMessage = "Postal code must be in length of 6 characters")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Cell Phone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(15, ErrorMessage = "Home phone can not exceed 15 characters")]
        [Phone]
        public string CellPhoneNumber { get; set; }


        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+((\.(\w){2,3})+)$)", ErrorMessage = "Please enter a valid email address")]
        [EmailAddress]
        public string OfficeEmail { get; set; }


        [Required(ErrorMessage = "The Date Of Birth is required.")]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [LegalAgeValidation(18, ErrorMessage = "The minimum age is 18 in order to become an agent")]
        public DateTime DOB { get; set; }

        [Required]
        [Display(Name = "Legal Age Confirmed")]
        [LegalAgeTerms(Value = true, ErrorMessage = "You must accept the age terms at least 18")]
        public bool Confirmed { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}