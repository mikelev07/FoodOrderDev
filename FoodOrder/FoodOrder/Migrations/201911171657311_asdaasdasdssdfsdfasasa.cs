namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdaasdasdssdfsdfasasa : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Garnishes", name: "DishId", newName: "Dish_Id");
            RenameIndex(table: "dbo.Garnishes", name: "IX_DishId", newName: "IX_Dish_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Garnishes", name: "IX_Dish_Id", newName: "IX_DishId");
            RenameColumn(table: "dbo.Garnishes", name: "Dish_Id", newName: "DishId");
        }
    }
}
