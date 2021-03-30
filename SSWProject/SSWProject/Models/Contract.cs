using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSWProject.Models
{
    public class Contract
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        //public ICollection<File> Files { get; set; }
    }
}