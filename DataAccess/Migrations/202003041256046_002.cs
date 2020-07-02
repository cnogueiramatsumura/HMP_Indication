namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _002 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chamada", "Observacao", c => c.String(maxLength: 500, unicode: false));
            DropColumn("dbo.Chamada", "Observarcao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Chamada", "Observarcao", c => c.String(maxLength: 500, unicode: false));
            DropColumn("dbo.Chamada", "Observacao");
        }
    }
}
