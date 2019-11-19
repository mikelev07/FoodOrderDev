namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdaasda : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChoosenDishes", "Dish_Id", "dbo.Dishes");
            DropIndex("dbo.ChoosenDishes", new[] { "Dish_Id" });
            AddColumn("dbo.ChoosenDishes", "DishName", c => c.String());
            AddColumn("dbo.ChoosenDishes", "DishCategoryName", c => c.String());
            AddColumn("dbo.ChoosenDishes", "Proteins", c => c.Double(nullable: false));
            AddColumn("dbo.ChoosenDishes", "Fats", c => c.Double(nullable: false));
            AddColumn("dbo.ChoosenDishes", "Carbonhydrates", c => c.Double(nullable: false));
            AddColumn("dbo.ChoosenDishes", "Kilocalories", c => c.Double(nullable: false));
            AddColumn("dbo.ChoosenDishes", "ImagePath", c => c.String());
            DropColumn("dbo.ChoosenDishes", "Dish_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChoosenDishes", "Dish_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.ChoosenDishes", "ImagePath");
            DropColumn("dbo.ChoosenDishes", "Kilocalories");
            DropColumn("dbo.ChoosenDishes", "Carbonhydrates");
            DropColumn("dbo.ChoosenDishes", "Fats");
            DropColumn("dbo.ChoosenDishes", "Proteins");
            DropColumn("dbo.ChoosenDishes", "DishCategoryName");
            DropColumn("dbo.ChoosenDishes", "DishName");
            CreateIndex("dbo.ChoosenDishes", "Dish_Id");
            AddForeignKey("dbo.ChoosenDishes", "Dish_Id", "dbo.Dishes", "Id");
        }
    }
}
