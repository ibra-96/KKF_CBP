namespace AlfaPeople.KingKhalidFoundation.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReminderDateToProjectImpactEvaluation3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectImpactEvaluations",
                c => new
                    {
                        ProjectImpactEvaluationId = c.Guid(nullable: false),
                        IncubationWorkshopID = c.Guid(nullable: false),
                        Deadline = c.DateTime(nullable: false, storeType: "date"),
                        ReminderDate = c.DateTime(storeType: "date"),
                        Notes = c.String(),
                        IsFilled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectImpactEvaluationId)
                .ForeignKey("dbo.IncubationWorkshops", t => t.IncubationWorkshopID)
                .Index(t => t.IncubationWorkshopID);
            
            CreateTable(
                "dbo.ProjectImpactEvaluationRequests",
                c => new
                    {
                        ProjectImpactEvaluationRequestId = c.Guid(nullable: false),
                        ProjectImpactEvaluationId = c.Guid(nullable: false),
                        FrontendUserId = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                        ReminderSentOn = c.DateTime(),
                        FilledOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProjectImpactEvaluationRequestId)
                .ForeignKey("dbo.ProjectImpactEvaluations", t => t.ProjectImpactEvaluationId)
                .ForeignKey("dbo.FrontendUsers", t => t.FrontendUserId)
                .Index(t => t.ProjectImpactEvaluationId)
                .Index(t => t.FrontendUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectImpactEvaluationRequests", "FrontendUserId", "dbo.FrontendUsers");
            DropForeignKey("dbo.ProjectImpactEvaluations", "IncubationWorkshopID", "dbo.IncubationWorkshops");
            DropForeignKey("dbo.ProjectImpactEvaluationRequests", "ProjectImpactEvaluationId", "dbo.ProjectImpactEvaluations");
            DropIndex("dbo.ProjectImpactEvaluationRequests", new[] { "FrontendUserId" });
            DropIndex("dbo.ProjectImpactEvaluationRequests", new[] { "ProjectImpactEvaluationId" });
            DropIndex("dbo.ProjectImpactEvaluations", new[] { "IncubationWorkshopID" });
            DropTable("dbo.ProjectImpactEvaluationRequests");
            DropTable("dbo.ProjectImpactEvaluations");
        }
    }
}
