namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUserViewModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Companies", "IsCheckInstruction");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "IsCheckInstruction", c => c.Boolean(nullable: false));
        }
    }
}
