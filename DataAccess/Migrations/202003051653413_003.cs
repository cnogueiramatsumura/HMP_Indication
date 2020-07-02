namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _003 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChamadaEditada", "NewGain", c => c.Decimal(nullable: false, precision: 18, scale: 8));
            AddColumn("dbo.ChamadaEditada", "NewLoss", c => c.Decimal(nullable: false, precision: 18, scale: 8));
            DropColumn("dbo.ChamadaEditada", "PrecoGain");
            DropColumn("dbo.ChamadaEditada", "PrecoLoss");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChamadaEditada", "PrecoLoss", c => c.Decimal(nullable: false, precision: 18, scale: 8));
            AddColumn("dbo.ChamadaEditada", "PrecoGain", c => c.Decimal(nullable: false, precision: 18, scale: 8));
            DropColumn("dbo.ChamadaEditada", "NewLoss");
            DropColumn("dbo.ChamadaEditada", "NewGain");
        }
    }
}
