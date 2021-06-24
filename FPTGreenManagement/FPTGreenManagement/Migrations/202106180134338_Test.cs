namespace FPTGreenManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Telephone", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "WorkingPlace", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "WorkingPlace");
            DropColumn("dbo.AspNetUsers", "Telephone");
            DropColumn("dbo.AspNetUsers", "Age");
            DropColumn("dbo.AspNetUsers", "Name");
        }
    }
}
