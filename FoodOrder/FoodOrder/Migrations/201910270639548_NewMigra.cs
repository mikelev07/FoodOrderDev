namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMigra : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserViewModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        Name = c.String(),
                        Surname = c.String(),
                        Patronymic = c.String(),
                        Age = c.Int(nullable: false),
                        CompanyId = c.String(maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        IsVisible = c.Boolean(),
                        PhoneNumber = c.String(),
                        HasOrderToday = c.Boolean(nullable: false),
                        IsCheckInstruction = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserViewModels", "CompanyId", "dbo.Companies");
            DropIndex("dbo.UserViewModels", new[] { "CompanyId" });
            DropTable("dbo.UserViewModels");
        }
    }
}
