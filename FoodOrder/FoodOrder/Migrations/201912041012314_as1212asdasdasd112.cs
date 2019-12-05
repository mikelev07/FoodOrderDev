namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class as1212asdasdasd112 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Packs", "GarnishesForPacks", c => c.String());
            AddColumn("dbo.Dishes", "GarnishesForPacks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dishes", "GarnishesForPacks");
            DropColumn("dbo.Packs", "GarnishesForPacks");
        }
    }
}
