namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdas : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GarnishForDishes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Dish_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            RenameColumn(table: "dbo.Dishes", name: "GarnishId", newName: "Dish_Id");
            AddColumn("dbo.Dishes", "GarnishId", c => c.String(maxLength: 128));
            CreateIndex("dbo.GarnishForDishes", "Dish_Id");
        }
    }
}
