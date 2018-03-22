namespace Hogwarts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAnswerState_AddColumn_Progress_UserderstandingLevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAnswerStates", "Progress", c => c.Int(nullable: false));
            AddColumn("dbo.UserAnswerStates", "UnderStandingLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserAnswerStates", "UnderStandingLevel");
            DropColumn("dbo.UserAnswerStates", "Progress");
        }
    }
}
