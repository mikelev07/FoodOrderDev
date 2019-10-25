namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DishCategories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Dishes", "DishCategoryId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Dishes", "DishCategoryId");
            AddForeignKey("dbo.Dishes", "DishCategoryId", "dbo.DishCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dishes", "DishCategoryId", "dbo.DishCategories");
            DropIndex("dbo.Dishes", new[] { "DishCategoryId" });
            DropColumn("dbo.Dishes", "DishCategoryId");
            DropTable("dbo.DishCategories");
        }
    }
}
