namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityAndCompanyMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Logotype = c.String(),
                        TypeOfPayment = c.Int(nullable: false),
                        UnlimitedOrders = c.Boolean(nullable: false),
                        RepresentativeId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.RepresentativeId)
                .Index(t => t.RepresentativeId);
            
            AddColumn("dbo.AspNetUsers", "Company_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "Company_Id");
            AddForeignKey("dbo.AspNetUsers", "Company_Id", "dbo.Companies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Companies", "RepresentativeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Company_Id", "dbo.Companies");
            DropIndex("dbo.AspNetUsers", new[] { "Company_Id" });
            DropIndex("dbo.Companies", new[] { "RepresentativeId" });
            DropColumn("dbo.AspNetUsers", "Company_Id");
            DropTable("dbo.Companies");
        }
    }
}
