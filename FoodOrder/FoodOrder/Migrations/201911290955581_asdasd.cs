namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderPack",
                c => new
                    {
                        OrderId = c.String(nullable: false, maxLength: 128),
                        PackId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.OrderId, t.PackId })
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Packs", t => t.PackId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.PackId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderPack", "PackId", "dbo.Packs");
            DropForeignKey("dbo.OrderPack", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderPack", new[] { "PackId" });
            DropIndex("dbo.OrderPack", new[] { "OrderId" });
            DropTable("dbo.OrderPack");
        }
    }
}
