namespace Hogwarts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupRecord : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Records", newName: "UserRecords");
            CreateTable(
                "dbo.GroupRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Role = c.String(),
                        LectureId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        Point = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GroupRecords");
            RenameTable(name: "dbo.UserRecords", newName: "Records");
        }
    }
}
