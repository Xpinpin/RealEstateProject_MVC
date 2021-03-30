using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSWProject.Models
{
    public class LegalAgeValidation : ValidationAttribute
    {
        public int LegalAge { get; set; }

        public LegalAgeValidation(int legalAge)
        {
            LegalAge = legalAge;
        }

        public override bool IsValid(object value)
        {
            return Convert.ToDateTime(value).AddYears(LegalAge).Year < DateTime.Now.Year;

        }
    }
}