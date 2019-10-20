namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequisitesChange : DbMigration
    {
        public override void Up()
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
            CreateIndex("dbo.Companies", "Requisites_Id");
            AddForeignKey("dbo.Companies", "Requisites_Id", "dbo.Requisites", "Id");
            DropColumn("dbo.Companies", "Requisites_FullName");
            DropColumn("dbo.Companies", "Requisites_BIN_IIN");
            DropColumn("dbo.Companies", "Requisites_IIK");
            DropColumn("dbo.Companies", "Requisites_RNN");
            DropColumn("dbo.Companies", "Requisites_BIK");
            DropColumn("dbo.Companies", "Requisites_Bank");
            DropColumn("dbo.Companies", "Requisites_LegalAddress");
            DropColumn("dbo.Companies", "Requisites_ActualAddress");
            DropColumn("dbo.Companies", "Requisites_Director");
            DropColumn("dbo.Companies", "Requisites_PhoneNumber");
            DropColumn("dbo.Companies", "Requisites_Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "Requisites_Email", c => c.String());
            AddColumn("dbo.Companies", "Requisites_PhoneNumber", c => c.String());
            AddColumn("dbo.Companies", "Requisites_Director", c => c.String());
            AddColumn("dbo.Companies", "Requisites_ActualAddress", c => c.String());
            AddColumn("dbo.Companies", "Requisites_LegalAddress", c => c.String());
            AddColumn("dbo.Companies", "Requisites_Bank", c => c.String());
            AddColumn("dbo.Companies", "Requisites_BIK", c => c.String());
            AddColumn("dbo.Companies", "Requisites_RNN", c => c.String());
            AddColumn("dbo.Companies", "Requisites_IIK", c => c.String());
            AddColumn("dbo.Companies", "Requisites_BIN_IIN", c => c.String());
            AddColumn("dbo.Companies", "Requisites_FullName", c => c.String());
            DropForeignKey("dbo.Companies", "Requisites_Id", "dbo.Requisites");
            DropIndex("dbo.Companies", new[] { "Requisites_Id" });
            DropColumn("dbo.Companies", "Requisites_Id");
            DropTable("dbo.Requisites");
        }
    }
}
