namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasd : DbMigration
    {
        public override void Up()
        {
            DropTable(name: "dbo.GarnishForDishes");
        }
        
        public override void Down()
        {
            DropTable(name: "dbo.GarnishForDishes");
        }
    }
}
