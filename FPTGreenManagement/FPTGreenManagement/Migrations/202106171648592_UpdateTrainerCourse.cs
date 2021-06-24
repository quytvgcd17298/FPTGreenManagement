namespace FPTGreenManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTrainerCourse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TrainerCourses", "TrainerId", "dbo.AspNetUsers");
            DropIndex("dbo.TrainerCourses", new[] { "TrainerId" });
            DropPrimaryKey("dbo.TrainerCourses");
            AddColumn("dbo.TrainerCourses", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.TrainerCourses", "TrainerId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.TrainerCourses", "Id");
            CreateIndex("dbo.TrainerCourses", "TrainerId");
            AddForeignKey("dbo.TrainerCourses", "TrainerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrainerCourses", "TrainerId", "dbo.AspNetUsers");
            DropIndex("dbo.TrainerCourses", new[] { "TrainerId" });
            DropPrimaryKey("dbo.TrainerCourses");
            AlterColumn("dbo.TrainerCourses", "TrainerId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.TrainerCourses", "Id");
            AddPrimaryKey("dbo.TrainerCourses", new[] { "TrainerId", "CourseId" });
            CreateIndex("dbo.TrainerCourses", "TrainerId");
            AddForeignKey("dbo.TrainerCourses", "TrainerId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
