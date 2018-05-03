namespace Hogwarts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateKnowledge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Knowledges", "CategoryId", c => c.String());
            AlterColumn("dbo.KnowledgeCategories", "CategoryName", c => c.String(nullable: false));
            DropColumn("dbo.Knowledges", "IncludeCategoryName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Knowledges", "IncludeCategoryName", c => c.String());
            AlterColumn("dbo.KnowledgeCategories", "CategoryName", c => c.String());
            DropColumn("dbo.Knowledges", "CategoryId");
        }
    }
}
