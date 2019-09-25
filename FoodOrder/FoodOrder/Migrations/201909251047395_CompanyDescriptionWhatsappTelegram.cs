namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyDescriptionWhatsappTelegram : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Whatsapp", c => c.String());
            AddColumn("dbo.Companies", "Telegram", c => c.String());
            AddColumn("dbo.Companies", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "Description");
            DropColumn("dbo.Companies", "Telegram");
            DropColumn("dbo.Companies", "Whatsapp");
        }
    }
}
