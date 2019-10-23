namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyModelChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Companies", "Requisites_Id", "dbo.Requisites");
            DropIndex("dbo.Companies", new[] { "Requisites_Id" });
            AddColumn("dbo.Companies", "FullName", c => c.String());
            AddColumn("dbo.Companies", "BIN_IIN", c => c.String());
            AddColumn("dbo.Companies", "IIK", c => c.String());
            AddColumn("dbo.Companies", "RNN", c => c.String());
            AddColumn("dbo.Companies", "BIK", c => c.String());
            AddColumn("dbo.Companies", "Bank", c => c.String());
            AddColumn("dbo.Companies", "LegalAddress", c => c.String());
            AddColumn("dbo.Companies", "ActualAddress", c => c.String());
            AddColumn("dbo.Companies", "Director", c => c.String());
            AddColumn("dbo.Companies", "PhoneNumber", c => c.String());
            AddColumn("dbo.Companies", "Email", c => c.String());
            DropColumn("dbo.Companies", "Requisites_Id");
            DropTable("dbo.Requisites");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Requisites",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        BIN_IIN = c.String(),
                        IIK = c.String(),
                        RNN = c.String(),
                        BIK = c.String(),
                        Bank = c.String(),
                        LegalAddress = c.String(),
                        ActualAddress = c.String(),
                        Director = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Companies", "Requisites_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.Companies", "Email");
            DropColumn("dbo.Companies", "PhoneNumber");
            DropColumn("dbo.Companies", "Director");
            DropColumn("dbo.Companies", "ActualAddress");
            DropColumn("dbo.Companies", "LegalAddress");
            DropColumn("dbo.Companies", "Bank");
            DropColumn("dbo.Companies", "BIK");
            DropColumn("dbo.Companies", "RNN");
            DropColumn("dbo.Companies", "IIK");
            DropColumn("dbo.Companies", "BIN_IIN");
            DropColumn("dbo.Companies", "FullName");
            CreateIndex("dbo.Companies", "Requisites_Id");
            AddForeignKey("dbo.Companies", "Requisites_Id", "dbo.Requisites", "Id");
        }
    }
}
