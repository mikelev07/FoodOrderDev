namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdaasasas : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Garnishes", name: "Dish_Id", newName: "DishId");
            RenameIndex(table: "dbo.Garnishes", name: "IX_Dish_Id", newName: "IX_DishId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Garnishes", name: "IX_DishId", newName: "IX_Dish_Id");
            RenameColumn(table: "dbo.Garnishes", name: "DishId", newName: "Dish_Id");
        }
    }
}
