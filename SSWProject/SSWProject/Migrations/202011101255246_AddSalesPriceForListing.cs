namespace SSWProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSalesPriceForListing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Listings", "SalesPrice", c => c.Double(nullable: false));
            DropForeignKey("dbo.ListingContracts", "ListingId", "dbo.Listings");
            DropForeignKey("dbo.ListingContracts", "AgentId", "dbo.Agents");
            DropTable("dbo.ListingContracts");

        }

        public override void Down()
        {
            DropColumn("dbo.Listings", "SalesPrice");
        }
    }
}
