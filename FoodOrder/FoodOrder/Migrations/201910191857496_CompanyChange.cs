namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Requisites_FullName", c => c.String());
            AddColumn("dbo.Companies", "Requisites_BIN_IIN", c => c.String());
            AddColumn("dbo.Companies", "Requisites_IIK", c => c.String());
            AddColumn("dbo.Companies", "Requisites_RNN", c => c.String());
            AddColumn("dbo.Companies", "Requisites_BIK", c => c.String());
            AddColumn("dbo.Companies", "Requisites_Bank", c => c.String());
            AddColumn("dbo.Companies", "Requisites_LegalAddress", c => c.String());
            AddColumn("dbo.Companies", "Requisites_ActualAddress", c => c.String());
            AddColumn("dbo.Companies", "Requisites_Director", c => c.String());
            AddColumn("dbo.Companies", "Requisites_PhoneNumber", c => c.String());
            AddColumn("dbo.Companies", "Requisites_Email", c => c.String());
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Patronymic", c => c.String());
            AddColumn("dbo.AspNetUsers", "Age", c => c.Int(nullable: false));
            DropColumn("dbo.Companies", "LogotypePath");
            DropColumn("dbo.Companies", "Requisites");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "Requisites", c => c.String());
            AddColumn("dbo.Companies", "LogotypePath", c => c.String());
            DropColumn("dbo.AspNetUsers", "Age");
            DropColumn("dbo.AspNetUsers", "Patronymic");
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.Companies", "Requisites_Email");
            DropColumn("dbo.Companies", "Requisites_PhoneNumber");
            DropColumn("dbo.Companies", "Requisites_Director");
            DropColumn("dbo.Companies", "Requisites_ActualAddress");
            DropColumn("dbo.Companies", "Requisites_LegalAddress");
            DropColumn("dbo.Companies", "Requisites_Bank");
            DropColumn("dbo.Companies", "Requisites_BIK");
            DropColumn("dbo.Companies", "Requisites_RNN");
            DropColumn("dbo.Companies", "Requisites_IIK");
            DropColumn("dbo.Companies", "Requisites_BIN_IIN");
            DropColumn("dbo.Companies", "Requisites_FullName");
        }
    }
}
