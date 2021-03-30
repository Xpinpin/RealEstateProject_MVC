using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSWProject.Models
{
    public class File
    {
        public int ID { get; set; }

        public string PathName { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public FileType FileType { get; set; }

        public FileStatus Status { get; set; }

        //public int ObjectId { get; set; }

        public int? AgentID { get; set; }
        //public virtual Agent Agent { get; set; }
        [NotMapped]
        public  IEnumerable<Agent> GetAgents { get; set; }

        //public int? ContractID { get; set; }

        //public virtual Contract Contract { get; set; }

        public int? ListingID { get; set; }
        public virtual Listing Listing { get; set; }
        [NotMapped]
        public IEnumerable<Contract> GetListings { get; set; }

        [NotMapped]
        public ICollection<File> Files { get; set; }
     
    }
}