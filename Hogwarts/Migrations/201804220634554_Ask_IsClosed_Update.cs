namespace Hogwarts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ask_IsClosed_Update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Asks", "IsClosed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Asks", "IsClosed", c => c.String());
        }
    }
}
