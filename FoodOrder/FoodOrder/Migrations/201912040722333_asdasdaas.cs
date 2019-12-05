namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasdaas : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Packs", "MenuId", "dbo.Menus");
            AddForeignKey("dbo.Packs", "MenuId", "dbo.Menus", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Packs", "MenuId", "dbo.Menus");
            AddForeignKey("dbo.Packs", "MenuId", "dbo.Menus", "Id");
        }
    }
}
