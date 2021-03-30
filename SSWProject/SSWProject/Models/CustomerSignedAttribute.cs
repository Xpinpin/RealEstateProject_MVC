using System;
using System.ComponentModel.DataAnnotations;

namespace SSWProject.Models
{
    internal class CustomerSignedAttribute : ValidationAttribute
    {
        public bool Value { get; set; }

        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value == Value;
        }
    }
}