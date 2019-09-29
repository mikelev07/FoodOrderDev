namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class andNeeew : DbMigration
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
                        Whatsapp = c.String(),
                        Telegram = c.String(),
                        Description = c.String(),
                        RepresentativeId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.RepresentativeId)
                .Index(t => t.RepresentativeId);
            
            CreateTable(
                "dbo.UserViewModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        SecondName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "CompanyId", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "CompanyId");
            AddForeignKey("dbo.AspNetUsers", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Companies", "RepresentativeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CompanyId", "dbo.Companies");
            DropIndex("dbo.AspNetUsers", new[] { "CompanyId" });
            DropIndex("dbo.Companies", new[] { "RepresentativeId" });
            DropColumn("dbo.AspNetUsers", "CompanyId");
            DropTable("dbo.UserViewModels");
            DropTable("dbo.Companies");
        }
    }
}
