using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSWProject.Models
{
    public class LegalAgeTerms : ValidationAttribute
    {
        public bool Value { get; set; }

        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value == Value;
        }

    }
}