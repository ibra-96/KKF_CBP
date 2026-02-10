namespace AlfaPeople.KingKhalidFoundation.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIncubationWorkshopControlOptions11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IncubationWorkshopOptionValues",
                c => new
                    {
                        ValueID = c.Guid(nullable: false),
                        OptionID = c.Guid(nullable: false),
                        TransID = c.Guid(),
                        TransValueID = c.Guid(nullable: false),
                        Value = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ValueID)
                .ForeignKey("dbo.IncubationWorkshopControlOptions", t => t.OptionID)
                .ForeignKey("dbo.IncubationWorkshopBLTransactionsValues", t => t.TransValueID, cascadeDelete: true)
                .Index(t => t.OptionID)
                .Index(t => t.TransValueID);
            
            CreateTable(
                "dbo.IncubationWorkshopControlOptions",
                c => new
                    {
                        OptionID = c.Guid(nullable: false),
                        OptionNameEn = c.String(nullable: false),
                        OptionNameAr = c.String(nullable: false),
                        TransID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.OptionID)
                .ForeignKey("dbo.IncubationWorkshopBLTransactions", t => t.TransID, cascadeDelete: true)
                .Index(t => t.TransID);
            
            AddColumn("dbo.IncubationWorkshopBLTransactions", "IncubationWorkshopID", c => c.Guid(nullable: false));
            AddColumn("dbo.IncubationWorkshopBLTransactions", "IncubationWorkshopControls_ControlsID", c => c.Guid());
            AlterColumn("dbo.IncubationWorkshopBLTransactions", "IsRequired", c => c.String(nullable: false));
            AlterColumn("dbo.IncubationWorkshopBLTransactions", "ViewList_Display", c => c.String(nullable: false));
            CreateIndex("dbo.IncubationWorkshopBLTransactions", "IncubationWorkshopID");
            CreateIndex("dbo.IncubationWorkshopBLTransactions", "IncubationWorkshopControls_ControlsID");
            AddForeignKey("dbo.IncubationWorkshopBLTransactions", "IncubationWorkshopID", "dbo.IncubationWorkshops", "IncubationWorkshopID", cascadeDelete: true);
            AddForeignKey("dbo.IncubationWorkshopBLTransactions", "IncubationWorkshopControls_ControlsID", "dbo.IncubationWorkshopControls", "ControlsID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IncubationWorkshopOptionValues", "TransValueID", "dbo.IncubationWorkshopBLTransactionsValues");
            DropForeignKey("dbo.IncubationWorkshopOptionValues", "OptionID", "dbo.IncubationWorkshopControlOptions");
            DropForeignKey("dbo.IncubationWorkshopControlOptions", "TransID", "dbo.IncubationWorkshopBLTransactions");
            DropForeignKey("dbo.IncubationWorkshopBLTransactions", "IncubationWorkshopControls_ControlsID", "dbo.IncubationWorkshopControls");
            DropForeignKey("dbo.IncubationWorkshopBLTransactions", "IncubationWorkshopID", "dbo.IncubationWorkshops");
            DropIndex("dbo.IncubationWorkshopControlOptions", new[] { "TransID" });
            DropIndex("dbo.IncubationWorkshopOptionValues", new[] { "TransValueID" });
            DropIndex("dbo.IncubationWorkshopOptionValues", new[] { "OptionID" });
            DropIndex("dbo.IncubationWorkshopBLTransactions", new[] { "IncubationWorkshopControls_ControlsID" });
            DropIndex("dbo.IncubationWorkshopBLTransactions", new[] { "IncubationWorkshopID" });
            AlterColumn("dbo.IncubationWorkshopBLTransactions", "ViewList_Display", c => c.Boolean(nullable: false));
            AlterColumn("dbo.IncubationWorkshopBLTransactions", "IsRequired", c => c.Boolean(nullable: false));
            DropColumn("dbo.IncubationWorkshopBLTransactions", "IncubationWorkshopControls_ControlsID");
            DropColumn("dbo.IncubationWorkshopBLTransactions", "IncubationWorkshopID");
            DropTable("dbo.IncubationWorkshopControlOptions");
            DropTable("dbo.IncubationWorkshopOptionValues");
        }
    }
}
