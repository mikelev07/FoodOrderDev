namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasdasd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Dishes", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Dishes", new[] { "Order_Id" });
            CreateTable(
                "dbo.ChoosenDishes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        GarinshName = c.String(),
                        Dish_Id = c.String(maxLength: 128),
                        Order_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dishes", t => t.Dish_Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.Dish_Id)
                .Index(t => t.Order_Id);
            
            DropColumn("dbo.Dishes", "Order_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dishes", "Order_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.ChoosenDishes", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.ChoosenDishes", "Dish_Id", "dbo.Dishes");
            DropIndex("dbo.ChoosenDishes", new[] { "Order_Id" });
            DropIndex("dbo.ChoosenDishes", new[] { "Dish_Id" });
            DropTable("dbo.ChoosenDishes");
            CreateIndex("dbo.Dishes", "Order_Id");
            AddForeignKey("dbo.Dishes", "Order_Id", "dbo.Orders", "Id");
        }
    }
}
