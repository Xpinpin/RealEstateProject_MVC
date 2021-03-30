namespace SSWProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 30),
                        MiddleName = c.String(maxLength: 10),
                        LastName = c.String(nullable: false, maxLength: 40),
                        LoggedInUserName = c.String(nullable: false, maxLength: 100),
                        StreetAddress = c.String(nullable: false, maxLength: 150),
                        Municipality = c.String(nullable: false, maxLength: 50),
                        Province = c.Int(nullable: false),
                        PostalCode = c.String(nullable: false, maxLength: 7),
                        HomePhoneNumber = c.String(nullable: false, maxLength: 15),
                        CellPhoneNumber = c.String(nullable: false, maxLength: 15),
                        OfficePhoneNumber = c.String(nullable: false, maxLength: 15),
                        OfficeEmail = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                        SIN = c.String(nullable: false, maxLength: 9),
                        Confirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AgentID = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Agents", t => t.AgentID, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.AgentID)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 30),
                        MiddleName = c.String(maxLength: 10),
                        LastName = c.String(nullable: false, maxLength: 40),
                        StreetAddress = c.String(nullable: false, maxLength: 150),
                        Municipality = c.String(nullable: false, maxLength: 50),
                        Province = c.Int(nullable: false),
                        PostalCode = c.String(nullable: false, maxLength: 7),
                        CellPhoneNumber = c.String(nullable: false, maxLength: 15),
                        OfficeEmail = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                        Confirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PathName = c.String(),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        AgentID = c.Int(),
                        ContractID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Agents", t => t.AgentID)
                .ForeignKey("dbo.Contracts", t => t.ContractID)
                .Index(t => t.AgentID)
                .Index(t => t.ContractID);
            
            CreateTable(
                "dbo.Contracts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "ContractID", "dbo.Contracts");
            DropForeignKey("dbo.Files", "AgentID", "dbo.Agents");
            DropForeignKey("dbo.Appointments", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Appointments", "AgentID", "dbo.Agents");
            DropIndex("dbo.Files", new[] { "ContractID" });
            DropIndex("dbo.Files", new[] { "AgentID" });
            DropIndex("dbo.Appointments", new[] { "CustomerID" });
            DropIndex("dbo.Appointments", new[] { "AgentID" });
            DropTable("dbo.Contracts");
            DropTable("dbo.Files");
            DropTable("dbo.Customers");
            DropTable("dbo.Appointments");
            DropTable("dbo.Agents");
        }
    }
}
