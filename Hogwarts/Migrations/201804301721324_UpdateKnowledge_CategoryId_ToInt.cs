namespace Hogwarts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateKnowledge_CategoryId_ToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Knowledges", "CategoryId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Knowledges", "CategoryId", c => c.String());
        }
    }
}
