namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServerConfig", "SmtpAdress", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ServerConfig", "SmtpPort", c => c.Int());
            AddColumn("dbo.ServerConfig", "SmtpUsername", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ServerConfig", "SmtpPassword", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServerConfig", "SmtpPassword");
            DropColumn("dbo.ServerConfig", "SmtpUsername");
            DropColumn("dbo.ServerConfig", "SmtpPort");
            DropColumn("dbo.ServerConfig", "SmtpAdress");
        }
    }
}
