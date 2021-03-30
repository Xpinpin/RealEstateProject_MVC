namespace SSWProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteRelaContracts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "ContractID", "dbo.Contracts");
            DropIndex("dbo.Files", new[] { "ContractID" });
            DropColumn("dbo.Files", "ContractID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "ContractID", c => c.Int());
            CreateIndex("dbo.Files", "ContractID");
            AddForeignKey("dbo.Files", "ContractID", "dbo.Contracts", "ID");
        }
    }
}
