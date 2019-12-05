namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasdasdasdasdasd112 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChoosenDishes", "Weight", c => c.Double(nullable: false));
            AddColumn("dbo.Dishes", "Weight", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dishes", "Weight");
            DropColumn("dbo.ChoosenDishes", "Weight");
        }
    }
}
