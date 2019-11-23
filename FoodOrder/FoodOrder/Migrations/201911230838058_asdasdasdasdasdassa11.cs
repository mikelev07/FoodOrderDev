namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasdasdasdasdassa11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "SingleChoice", c => c.Boolean(nullable: false));
            AddColumn("dbo.Companies", "MultiChoice", c => c.Boolean(nullable: false));
            AddColumn("dbo.Companies", "PacksPicker", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "PacksPicker");
            DropColumn("dbo.Companies", "MultiChoice");
            DropColumn("dbo.Companies", "SingleChoice");
        }
    }
}
