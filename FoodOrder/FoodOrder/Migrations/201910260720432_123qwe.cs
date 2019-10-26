namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _123qwe : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dishes", "Menu_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Dishes", "Menu_Id");
            AddForeignKey("dbo.Dishes", "Menu_Id", "dbo.Menus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dishes", "Menu_Id", "dbo.Menus");
            DropIndex("dbo.Dishes", new[] { "Menu_Id" });
            DropColumn("dbo.Dishes", "Menu_Id");
        }
    }
}
