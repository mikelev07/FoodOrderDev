namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderModelMigration : DbMigration
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
            
            AddColumn("dbo.Dishes", "Order_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Dishes", "Order_Id");
            AddForeignKey("dbo.Dishes", "Order_Id", "dbo.Orders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Dishes", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Dishes", new[] { "Order_Id" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropColumn("dbo.Dishes", "Order_Id");
            DropTable("dbo.Orders");
        }
    }
}
