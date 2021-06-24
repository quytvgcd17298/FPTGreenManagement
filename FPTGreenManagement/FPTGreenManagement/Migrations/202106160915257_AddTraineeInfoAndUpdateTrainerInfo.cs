namespace FPTGreenManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTraineeInfoAndUpdateTrainerInfo : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Trainers", name: "UserId", newName: "Id");
            RenameIndex(table: "dbo.Trainers", name: "IX_UserId", newName: "IX_Id");
            CreateTable(
                "dbo.Trainees",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TraineeName = c.String(),
                        Age = c.Int(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        Education = c.String(),
                        MainProgrammingLanguage = c.String(),
                        TOEICscore = c.Single(nullable: false),
                        ExperienceDetails = c.String(),
                        Department = c.String(),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trainees", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Trainees", new[] { "Id" });
            DropTable("dbo.Trainees");
            RenameIndex(table: "dbo.Trainers", name: "IX_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.Trainers", name: "Id", newName: "UserId");
        }
    }
}
