namespace FPTGreenManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrainerCourseAssign : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrainerCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrainerId = c.String(maxLength: 128),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Trainers", t => t.TrainerId)
                .Index(t => t.TrainerId)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrainerCourses", "TrainerId", "dbo.Trainers");
            DropForeignKey("dbo.TrainerCourses", "CourseId", "dbo.Courses");
            DropIndex("dbo.TrainerCourses", new[] { "CourseId" });
            DropIndex("dbo.TrainerCourses", new[] { "TrainerId" });
            DropTable("dbo.TrainerCourses");
        }
    }
}
