namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasdasdads : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ChoosenDishes", name: "Order_Id", newName: "OrderId");
            RenameIndex(table: "dbo.ChoosenDishes", name: "IX_Order_Id", newName: "IX_OrderId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ChoosenDishes", name: "IX_OrderId", newName: "IX_Order_Id");
            RenameColumn(table: "dbo.ChoosenDishes", name: "OrderId", newName: "Order_Id");
        }
    }
}
