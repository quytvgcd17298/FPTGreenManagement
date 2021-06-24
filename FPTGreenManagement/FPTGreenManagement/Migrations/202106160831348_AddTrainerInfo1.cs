namespace FPTGreenManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrainerInfo1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Trainers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        TrainerName = c.String(),
                        WorkingPlace = c.String(),
                        Telephone = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trainers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Trainers", new[] { "UserId" });
            DropTable("dbo.Trainers");
        }
    }
}
