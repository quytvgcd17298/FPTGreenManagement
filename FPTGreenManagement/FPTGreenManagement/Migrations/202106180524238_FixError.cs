namespace FPTGreenManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixError : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.AspNetUsers", "Age");
            DropColumn("dbo.AspNetUsers", "Telephone");
            DropColumn("dbo.AspNetUsers", "WorkingPlace");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "WorkingPlace", c => c.String());
            AddColumn("dbo.AspNetUsers", "Telephone", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
        }
    }
}
