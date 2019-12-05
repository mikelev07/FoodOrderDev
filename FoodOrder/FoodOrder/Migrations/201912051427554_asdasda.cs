namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasda : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "OrdersCountToday", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "OrdersCountToday");
        }
    }
}
