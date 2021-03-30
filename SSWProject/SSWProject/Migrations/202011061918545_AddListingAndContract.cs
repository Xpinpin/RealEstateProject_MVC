namespace SSWProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddListingAndContract : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ListingContracts",
                c => new
                    {
                        AgentId = c.Int(nullable: false),
                        ListingId = c.Int(nullable: false),
                        SalesPrice = c.Double(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsSigned = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.AgentId, t.ListingId })
                .ForeignKey("dbo.Agents", t => t.AgentId, cascadeDelete: true)
                .ForeignKey("dbo.Listings", t => t.ListingId, cascadeDelete: true)
                .Index(t => t.AgentId)
                .Index(t => t.ListingId);
            
            CreateTable(
                "dbo.Listings",
                c => new
                    {
                        ListingID = c.Int(nullable: false, identity: true),
                        StreetAddress = c.String(nullable: false, maxLength: 150),
                        Municipality = c.String(nullable: false, maxLength: 50),
                        province = c.Int(nullable: false),
                        OtherAddress = c.String(),
                        SquareFootage = c.Double(nullable: false),
                        BedsNum = c.Int(nullable: false),
                        BathsNum = c.Int(nullable: false),
                        StoriesNum = c.Int(nullable: false),
                        CityArea = c.String(nullable: false),
                        FeaturesSummary = c.String(),
                        HeatingType = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ListingID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.CustomerID);
            
            AddColumn("dbo.Files", "Listing_ListingID", c => c.Int());
            CreateIndex("dbo.Files", "Listing_ListingID");
            AddForeignKey("dbo.Files", "Listing_ListingID", "dbo.Listings", "ListingID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListingContracts", "ListingId", "dbo.Listings");
            DropForeignKey("dbo.Files", "Listing_ListingID", "dbo.Listings");
            DropForeignKey("dbo.Listings", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.ListingContracts", "AgentId", "dbo.Agents");
            DropIndex("dbo.Listings", new[] { "CustomerID" });
            DropIndex("dbo.ListingContracts", new[] { "ListingId" });
            DropIndex("dbo.ListingContracts", new[] { "AgentId" });
            DropIndex("dbo.Files", new[] { "Listing_ListingID" });
            DropColumn("dbo.Files", "Listing_ListingID");
            DropTable("dbo.Listings");
            DropTable("dbo.ListingContracts");
        }
    }
}
