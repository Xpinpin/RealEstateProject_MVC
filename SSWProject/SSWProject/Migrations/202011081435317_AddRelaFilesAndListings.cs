namespace SSWProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelaFilesAndListings : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Files", name: "Listing_ListingID", newName: "ListingID");
            RenameIndex(table: "dbo.Files", name: "IX_Listing_ListingID", newName: "IX_ListingID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Files", name: "IX_ListingID", newName: "IX_Listing_ListingID");
            RenameColumn(table: "dbo.Files", name: "ListingID", newName: "Listing_ListingID");
        }
    }
}
