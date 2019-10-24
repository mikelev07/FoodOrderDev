namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNewMigra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsCheckInstruction", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsCheckInstruction");
        }
    }
}
