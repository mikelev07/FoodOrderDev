namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasdasdas : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Garnishes", "DishId", "dbo.Dishes");
            DropIndex("dbo.Garnishes", new[] { "DishId" });
            CreateTable(
                "dbo.DishGarnish",
                c => new
                    {
                        DishId = c.String(nullable: false, maxLength: 128),
                        GarnishId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.DishId, t.GarnishId })
                .ForeignKey("dbo.Dishes", t => t.DishId, cascadeDelete: true)
                .ForeignKey("dbo.Garnishes", t => t.GarnishId, cascadeDelete: true)
                .Index(t => t.DishId)
                .Index(t => t.GarnishId);
            
            DropColumn("dbo.Garnishes", "DishId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Garnishes", "DishId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.DishGarnish", "GarnishId", "dbo.Garnishes");
            DropForeignKey("dbo.DishGarnish", "DishId", "dbo.Dishes");
            DropIndex("dbo.DishGarnish", new[] { "GarnishId" });
            DropIndex("dbo.DishGarnish", new[] { "DishId" });
            DropTable("dbo.DishGarnish");
            CreateIndex("dbo.Garnishes", "DishId");
            AddForeignKey("dbo.Garnishes", "DishId", "dbo.Dishes", "Id");
        }
    }
}
