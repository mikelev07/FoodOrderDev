namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletionOfComplexDishModel : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Dishes", name: "Garnish_Id", newName: "GarnishId");
            RenameIndex(table: "dbo.Dishes", name: "IX_Garnish_Id", newName: "IX_GarnishId");
            DropColumn("dbo.Dishes", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dishes", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            RenameIndex(table: "dbo.Dishes", name: "IX_GarnishId", newName: "IX_Garnish_Id");
            RenameColumn(table: "dbo.Dishes", name: "GarnishId", newName: "Garnish_Id");
        }
    }
}
