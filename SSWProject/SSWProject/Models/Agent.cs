using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SSWProject.Models
{
    public class Agent
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
        [Display(Name = "Agent Name")]
        public string AgentName
        {
            get
            {
                return FirstName + " " + MiddleName + " " + LastName;
            }
        }


        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + MiddleName + LastName;
            }
        }

        [Required]
        [Display(Name = "Username")]
        [StringLength(100, ErrorMessage = "The username can not exceed 100 characters")]
        [ValidateUserName(ErrorMessage = "User name has been already existed! Please choose another one")]
        [UsernameNoSpace(ErrorMessage = "User name can not have spaces in it")]
        public string LoggedInUserName { get; set; }


        [Required]
        [Display(Name = "Street Name")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Street Address must be in length of between 2 and 150 characters")]
        public string StreetAddress { get; set; }


        [Required]
        [Display(Name = "City")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "City must be in length of between 2 and 25 characters")]
        public string Municipality { get; set; }


        [Required]
        [Display(Name = "Province")]
        public Province? Province { get; set; }

        [NotMapped]
        public ICollection<Province> Provinces { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        [DataType(DataType.PostalCode)]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$", ErrorMessage = "Postal Code must be in correct format(Ex:E1G 2H5)")]
        [StringLength(7, ErrorMessage = "Postal code must be in length of 6 characters")]
        public string PostalCode { get; set; }


        [Required]
        [Display(Name = "Home Phone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(15, ErrorMessage = "Home phone can not exceed 15 characters")]
        [Phone]
        public string HomePhoneNumber { get; set; }


        [Required]
        [Display(Name = "Cell Phone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(15, ErrorMessage = "Cell phone can not exceed 15 characters")]
        [Phone]
        public string CellPhoneNumber { get; set; }

        [Required]
        [Display(Name = "Office Phone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(15, ErrorMessage = "Office phone can not exceed 15 characters")]
        [Phone]
        public string OfficePhoneNumber { get; set; }


        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+((\.(\w){2,3})+)$)", ErrorMessage = "Please enter a valid email address")]
        [EmailAddress]
        public string OfficeEmail { get; set; }


        [Required(ErrorMessage = "The Date Of Birth is required.")]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [LegalAgeValidation(18, ErrorMessage = "The minimum age is 19 in order to become an agent")]
        public DateTime DOB { get; set; }


        [Required]
        [Display(Name = "Social Insurance Number")]
        [StringLength(9)]
        [RegularExpression(@"^(\d{3}-\d{3}-\d{3})|(\d{9})$", ErrorMessage = "Please enter a valid SIN number with 9-digit")]
        
        public string SIN { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public ICollection<File> Files { get; set; }


        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        [Required]
        [Display(Name = "Legal Age Confirmed")]
        [LegalAgeTerms(Value = true, ErrorMessage = "You must accept the age terms at least 19")]
        public bool Confirmed { get; set; }

    }
}