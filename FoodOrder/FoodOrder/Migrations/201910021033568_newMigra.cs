namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newMigra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "LogotypePath", c => c.String());
            AddColumn("dbo.Companies", "GeneratedPassword", c => c.String());
            AddColumn("dbo.Companies", "Requisites", c => c.String());
            AddColumn("dbo.Orders", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Companies", "Logotype");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "Logotype", c => c.String());
            DropColumn("dbo.Orders", "Status");
            DropColumn("dbo.Companies", "Requisites");
            DropColumn("dbo.Companies", "GeneratedPassword");
            DropColumn("dbo.Companies", "LogotypePath");
        }
    }
}
