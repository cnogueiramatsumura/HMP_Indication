namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _017 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Analista", "Email", c => c.String(nullable: false, maxLength: 150, unicode: false));
            AddColumn("dbo.Analista", "Sobrenome", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Analista", "Password", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.Analista", "Foto", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Analista", "Ativo", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Analista", "Nome", c => c.String(nullable: false, maxLength: 150, unicode: false));            
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Analista", "Nome", c => c.String(maxLength: 150, unicode: false));
            DropColumn("dbo.Analista", "Ativo");
            DropColumn("dbo.Analista", "Foto");
            DropColumn("dbo.Analista", "Password");
            DropColumn("dbo.Analista", "Sobrenome");
            DropColumn("dbo.Analista", "Email");
        }
    }
}
