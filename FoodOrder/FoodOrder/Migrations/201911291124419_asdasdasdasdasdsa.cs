namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasdasdasdasdsa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderSpecId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "OrderSpecId");
        }
    }
}
