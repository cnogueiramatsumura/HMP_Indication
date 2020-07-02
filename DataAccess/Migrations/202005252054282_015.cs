namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _015 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecuperarSenha", "Utilizado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecuperarSenha", "Utilizado");
        }
    }
}
