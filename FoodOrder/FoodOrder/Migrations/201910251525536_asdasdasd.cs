namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasdasd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dishes", "TypeOfDish", c => c.Int(nullable: false));
            DropColumn("dbo.Dishes", "DateOfCreation");
            DropColumn("dbo.DishCategories", "DateOfCreation");
            DropColumn("dbo.Packs", "DateOfCreation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Packs", "DateOfCreation", c => c.DateTime(nullable: false));
            AddColumn("dbo.DishCategories", "DateOfCreation", c => c.DateTime(nullable: false));
            AddColumn("dbo.Dishes", "DateOfCreation", c => c.DateTime(nullable: false));
            DropColumn("dbo.Dishes", "TypeOfDish");
        }
    }
}
