namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasda : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DishStatistics", "DishCategoryName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DishStatistics", "DishCategoryName");
        }
    }
}
