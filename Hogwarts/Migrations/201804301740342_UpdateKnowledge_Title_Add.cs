namespace Hogwarts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateKnowledge_Title_Add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Knowledges", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Knowledges", "Title");
        }
    }
}
