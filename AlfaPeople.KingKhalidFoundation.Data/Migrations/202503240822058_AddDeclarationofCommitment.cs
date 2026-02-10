namespace AlfaPeople.KingKhalidFoundation.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationofCommitment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IncubationWorkshopAttachments", "IsCommitmentFile", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IncubationWorkshopAttachments", "IsCommitmentFile");
        }
    }
}
