namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasdasd1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DishStatistics",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DishName = c.String(),
                        Count = c.Int(nullable: false),
                        IsReady = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DishStatistics");
        }
    }
}
