namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DishModerChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dishes", "Proteins", c => c.Double(nullable: false));
            AddColumn("dbo.Dishes", "Fats", c => c.Double(nullable: false));
            AddColumn("dbo.Dishes", "Carbonhydrates", c => c.Double(nullable: false));
            AddColumn("dbo.Dishes", "Kilocalories", c => c.Double(nullable: false));
            DropColumn("dbo.Dishes", "NutritionalValue_Proteins");
            DropColumn("dbo.Dishes", "NutritionalValue_Fats");
            DropColumn("dbo.Dishes", "NutritionalValue_Carbonhydrates");
            DropColumn("dbo.Dishes", "NutritionalValue_Kilocalories");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dishes", "NutritionalValue_Kilocalories", c => c.Double(nullable: false));
            AddColumn("dbo.Dishes", "NutritionalValue_Carbonhydrates", c => c.Double(nullable: false));
            AddColumn("dbo.Dishes", "NutritionalValue_Fats", c => c.Double(nullable: false));
            AddColumn("dbo.Dishes", "NutritionalValue_Proteins", c => c.Double(nullable: false));
            DropColumn("dbo.Dishes", "Kilocalories");
            DropColumn("dbo.Dishes", "Carbonhydrates");
            DropColumn("dbo.Dishes", "Fats");
            DropColumn("dbo.Dishes", "Proteins");
        }
    }
}
