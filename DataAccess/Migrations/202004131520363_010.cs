namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _010 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ordem", "VendidaMercado", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ordem", "VendidaMercado");
        }
    }
}
