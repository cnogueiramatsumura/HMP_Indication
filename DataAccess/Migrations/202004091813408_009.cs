namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _009 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ordem", "PrecoVendaMercado", c => c.Decimal(precision: 18, scale: 8));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ordem", "PrecoVendaMercado", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
