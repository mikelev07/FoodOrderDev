namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasdasd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Garnishes", "Dish_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Garnishes", "Dish_Id");
            AddForeignKey("dbo.Garnishes", "Dish_Id", "dbo.Dishes", "Id");
            DropColumn("dbo.Dishes", "GarnishesForPacks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dishes", "GarnishesForPacks", c => c.String());
            DropForeignKey("dbo.Garnishes", "Dish_Id", "dbo.Dishes");
            DropIndex("dbo.Garnishes", new[] { "Dish_Id" });
            DropColumn("dbo.Garnishes", "Dish_Id");
        }
    }
}
