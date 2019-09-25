namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityModelBuilderMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Company_Id", "dbo.Companies");
            RenameColumn(table: "dbo.AspNetUsers", name: "Company_Id", newName: "CompanyId");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Company_Id", newName: "IX_CompanyId");
            AddForeignKey("dbo.AspNetUsers", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "CompanyId", "dbo.Companies");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_CompanyId", newName: "IX_Company_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "CompanyId", newName: "Company_Id");
            AddForeignKey("dbo.AspNetUsers", "Company_Id", "dbo.Companies", "Id");
        }
    }
}
