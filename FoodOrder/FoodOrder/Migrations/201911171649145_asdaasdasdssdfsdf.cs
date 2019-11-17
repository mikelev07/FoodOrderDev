namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdaasdasdssdfsdf : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Dishes", "Dish_Id");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dishes", "Dish_Id");
        }
    }
}
