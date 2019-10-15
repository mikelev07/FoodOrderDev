namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WhyWeStillHereJustToSuffer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Dishes", "Menu_Id", "dbo.Menus");
            DropIndex("dbo.Dishes", new[] { "Menu_Id" });
            RenameColumn(table: "dbo.Dishes", name: "GetDish_Id", newName: "Garnish_Id");
            RenameIndex(table: "dbo.Dishes", name: "IX_GetDish_Id", newName: "IX_Garnish_Id");
            CreateTable(
                "dbo.Packs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Drink_Id = c.String(maxLength: 128),
                        MainDish_Id = c.String(maxLength: 128),
                        Salad_Id = c.String(maxLength: 128),
                        Second_Id = c.String(maxLength: 128),
                        Menu_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dishes", t => t.Drink_Id)
                .ForeignKey("dbo.Dishes", t => t.MainDish_Id)
                .ForeignKey("dbo.Dishes", t => t.Salad_Id)
                .ForeignKey("dbo.Dishes", t => t.Second_Id)
                .ForeignKey("dbo.Menus", t => t.Menu_Id)
                .Index(t => t.Drink_Id)
                .Index(t => t.MainDish_Id)
                .Index(t => t.Salad_Id)
                .Index(t => t.Second_Id)
                .Index(t => t.Menu_Id);
            
            AddColumn("dbo.Orders", "Price", c => c.Double(nullable: false));
            AddColumn("dbo.Dishes", "Price", c => c.Double(nullable: false));
            DropColumn("dbo.Dishes", "Menu_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dishes", "Menu_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Packs", "Menu_Id", "dbo.Menus");
            DropForeignKey("dbo.Packs", "Second_Id", "dbo.Dishes");
            DropForeignKey("dbo.Packs", "Salad_Id", "dbo.Dishes");
            DropForeignKey("dbo.Packs", "MainDish_Id", "dbo.Dishes");
            DropForeignKey("dbo.Packs", "Drink_Id", "dbo.Dishes");
            DropIndex("dbo.Packs", new[] { "Menu_Id" });
            DropIndex("dbo.Packs", new[] { "Second_Id" });
            DropIndex("dbo.Packs", new[] { "Salad_Id" });
            DropIndex("dbo.Packs", new[] { "MainDish_Id" });
            DropIndex("dbo.Packs", new[] { "Drink_Id" });
            DropColumn("dbo.Dishes", "Price");
            DropColumn("dbo.Orders", "Price");
            DropTable("dbo.Packs");
            RenameIndex(table: "dbo.Dishes", name: "IX_Garnish_Id", newName: "IX_GetDish_Id");
            RenameColumn(table: "dbo.Dishes", name: "Garnish_Id", newName: "GetDish_Id");
            CreateIndex("dbo.Dishes", "Menu_Id");
            AddForeignKey("dbo.Dishes", "Menu_Id", "dbo.Menus", "Id");
        }
    }
}
