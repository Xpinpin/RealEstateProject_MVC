using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSWProject.Models
{
    public class ValidateUserName : ValidationAttribute
    {
        RealEstateContext db;

        public ValidateUserName()
        {
            db = new RealEstateContext();
        }

        public override bool IsValid(object value)
        {
            return db.Agents.Where(u => u.LoggedInUserName == value.ToString()).Count() <= 0;

        }
    }

    public class UsernameNoSpace : ValidationAttribute
    {
        RealEstateContext db;

        public UsernameNoSpace()
        {
            db = new RealEstateContext();
        }

        public override bool IsValid(object value)
        {
            return !value.ToString().Contains(" ");

        }
    }
}