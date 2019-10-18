namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityCompanyChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "RegistrationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "RegistrationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "NotActual", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "NotActual");
            DropColumn("dbo.AspNetUsers", "RegistrationDate");
            DropColumn("dbo.Companies", "RegistrationDate");
        }
    }
}
