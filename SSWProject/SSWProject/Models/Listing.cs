using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SSWProject.Models
{
    public class Listing
    {
        [Required]
        public int ListingID { get; set; }

        [Required]
        [Display(Name = "Street Name")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Street Address must be in length of between 2 and 150 characters.")]
        public string StreetAddress { get; set; }

        [Required]
        [Display(Name = "City")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "City must be in length of between 2 and 25 characters.")]
        public string Municipality { get; set; }

        [Required]
        [Display(Name = "Province")]
        public Province province { get; set; }

        [Display(Name = "Other Address")]
        public string OtherAddress { get; set; }

        [Required]
        [Display(Name = "Square Footage")]
        [Range(0, double.MaxValue, ErrorMessage = "You need to enter a valid number for square footage.")]
        public double SquareFootage { get; set; }

        [Required]
        [Display(Name = "Number Of Beds")]
        [Range(0, int.MaxValue, ErrorMessage = "You need to enter a valid number for number of beds.")]
        public int BedsNum { get; set; }

        [Required]
        [Display(Name = "Number Of Baths")]
        [Range(0, int.MaxValue, ErrorMessage = "You need to enter a valid number for number of baths.")]
        public int BathsNum { get; set; }

        [Required]
        [Display(Name = "Number Of stories")]
        [Range(0, int.MaxValue, ErrorMessage = "You need to enter a valid number for number of stories.")]
        public int StoriesNum { get; set; }

        [Required]
        [Display(Name = "The Area Of The City")]
        public string CityArea { get; set; }

        [Display(Name = "Special Features")]
        public string FeaturesSummary { get; set; }

        [Required]
        [Display(Name = "Type Of Heating")]
        public HeatingType HeatingType { get; set; }

        [Required]
        [Display(Name = "Sales Price")]
        [Range(0, double.MaxValue, ErrorMessage = "You must enter a valid number for sales price")]
        public double SalesPrice { get; set; }

        public int CustomerID { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<ListingContract> ListingContracts { get; set; }

        [NotMapped]
        [Display(Name = "Customer Name")]
        public string CustomerName
        {
            get
            {
                return Customer.FirstName + " " + Customer.LastName;
            }
        }

        [NotMapped]
        public string ListingOfCustomer
        {
            get
            {
                return Customer.FirstName + "" + Customer.LastName + "_" + StreetAddress;
            }
        }
    }
}