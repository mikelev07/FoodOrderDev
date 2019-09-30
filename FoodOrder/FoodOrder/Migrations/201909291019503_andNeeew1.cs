namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class andNeeew1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateOfCreation = c.DateTime(nullable: false),
                        Description = c.String(),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Dishes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        TypeOfDish = c.Int(nullable: false),
                        NutritionalValue_Proteins = c.Double(nullable: false),
                        NutritionalValue_Fats = c.Double(nullable: false),
                        NutritionalValue_Carbonhydrates = c.Double(nullable: false),
                        NutritionalValue_Kilocalories = c.Double(nullable: false),
                        ImagePath = c.String(),
                        HasGarnish = c.Boolean(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        GetDish_Id = c.String(maxLength: 128),
                        Order_Id = c.String(maxLength: 128),
                        Menu_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dishes", t => t.GetDish_Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .ForeignKey("dbo.Menus", t => t.Menu_Id)
                .Index(t => t.GetDish_Id)
                .Index(t => t.Order_Id)
                .Index(t => t.Menu_Id);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        DateOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dishes", "Menu_Id", "dbo.Menus");
            DropForeignKey("dbo.Orders", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Dishes", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Dishes", "GetDish_Id", "dbo.Dishes");
            DropIndex("dbo.Dishes", new[] { "Menu_Id" });
            DropIndex("dbo.Dishes", new[] { "Order_Id" });
            DropIndex("dbo.Dishes", new[] { "GetDish_Id" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropTable("dbo.Menus");
            DropTable("dbo.Dishes");
            DropTable("dbo.Orders");
        }
    }
}
