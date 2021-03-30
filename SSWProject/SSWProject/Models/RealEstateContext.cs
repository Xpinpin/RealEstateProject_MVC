using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SSWProject.Models
{
    public class RealEstateContext : DbContext
    {
        public RealEstateContext()
        {

        }        

        public DbSet<Agent> Agents { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Contract> Contracts { get; set; }

        public DbSet<File> Files { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<ListingContract> ListingContracts { get; set; }
    }
}