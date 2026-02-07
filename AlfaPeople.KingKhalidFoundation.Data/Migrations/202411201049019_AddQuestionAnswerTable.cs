namespace AlfaPeople.KingKhalidFoundation.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuestionAnswerTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckboxOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        OptionText = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionModels", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CheckboxOptions", "QuestionId", "dbo.QuestionModels");
            DropIndex("dbo.CheckboxOptions", new[] { "QuestionId" });
            DropTable("dbo.CheckboxOptions");
        }
    }
}
