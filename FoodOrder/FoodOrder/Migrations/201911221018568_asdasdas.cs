namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasdas : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Dishes", "Pack_Id", "dbo.Packs");
            DropIndex("dbo.Dishes", new[] { "Pack_Id" });
            RenameColumn(table: "dbo.Packs", name: "Menu_Id", newName: "MenuId");
            RenameIndex(table: "dbo.Packs", name: "IX_Menu_Id", newName: "IX_MenuId");
            CreateTable(
                "dbo.DishPack",
                c => new
                    {
                        DishId = c.String(nullable: false, maxLength: 128),
                        PackId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.DishId, t.PackId })
                .ForeignKey("dbo.Dishes", t => t.DishId, cascadeDelete: true)
                .ForeignKey("dbo.Packs", t => t.PackId, cascadeDelete: true)
                .Index(t => t.DishId)
                .Index(t => t.PackId);
            
            DropColumn("dbo.Dishes", "Pack_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dishes", "Pack_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.DishPack", "PackId", "dbo.Packs");
            DropForeignKey("dbo.DishPack", "DishId", "dbo.Dishes");
            DropIndex("dbo.DishPack", new[] { "PackId" });
            DropIndex("dbo.DishPack", new[] { "DishId" });
            DropTable("dbo.DishPack");
            RenameIndex(table: "dbo.Packs", name: "IX_MenuId", newName: "IX_Menu_Id");
            RenameColumn(table: "dbo.Packs", name: "MenuId", newName: "Menu_Id");
            CreateIndex("dbo.Dishes", "Pack_Id");
            AddForeignKey("dbo.Dishes", "Pack_Id", "dbo.Packs", "Id");
        }
    }
}
