namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnotherOne : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Surname", c => c.String());
            DropColumn("dbo.AspNetUsers", "SecondName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "SecondName", c => c.String());
            DropColumn("dbo.AspNetUsers", "Surname");
        }
    }
}
