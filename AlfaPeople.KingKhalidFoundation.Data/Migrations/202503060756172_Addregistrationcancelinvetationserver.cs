namespace AlfaPeople.KingKhalidFoundation.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addregistrationcancelinvetationserver : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IncubationPrivateInvitations", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.WorkshopPrivateInvitations", "UpdatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkshopPrivateInvitations", "UpdatedDate");
            DropColumn("dbo.IncubationPrivateInvitations", "UpdatedDate");
        }
    }
}
