namespace SSWProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeListingContract : DbMigration
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
            DropPrimaryKey("dbo.ListingContracts");
            AddColumn("dbo.ListingContracts", "ContractID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ListingContracts", "ContractID");
            DropColumn("dbo.ListingContracts", "SalesPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ListingContracts", "SalesPrice", c => c.Double(nullable: false));
            DropPrimaryKey("dbo.ListingContracts");
            DropColumn("dbo.ListingContracts", "ContractID");
            AddPrimaryKey("dbo.ListingContracts", new[] { "AgentId", "ListingId" });
        }
    }
}
