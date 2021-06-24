namespace FPTGreenManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserInCourse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Courses", new[] { "UserId" });
            AlterColumn("dbo.Courses", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Courses", "UserId");
            AddForeignKey("dbo.Courses", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Courses", new[] { "UserId" });
            AlterColumn("dbo.Courses", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Courses", "UserId");
            AddForeignKey("dbo.Courses", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
