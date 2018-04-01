namespace Hogwarts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableTargetTraineeOfLecture : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TargetTraineeOfLectures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LectureId = c.Int(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TargetTraineeOfLectures");
        }
    }
}
