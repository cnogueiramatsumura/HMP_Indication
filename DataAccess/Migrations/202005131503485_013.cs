namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _013 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.filters", "minPrice", c => c.Decimal(precision: 22, scale: 8));
            AlterColumn("dbo.filters", "maxPrice", c => c.Decimal(precision: 22, scale: 8));
            AlterColumn("dbo.filters", "tickSize", c => c.Decimal(precision: 22, scale: 8));
            AlterColumn("dbo.filters", "multiplierUp", c => c.Decimal(precision: 22, scale: 8));
            AlterColumn("dbo.filters", "multiplierDown", c => c.Decimal(precision: 22, scale: 8));
            AlterColumn("dbo.filters", "minQty", c => c.Decimal(precision: 22, scale: 8));
            AlterColumn("dbo.filters", "maxQty", c => c.Decimal(precision: 22, scale: 8));
            AlterColumn("dbo.filters", "stepSize", c => c.Decimal(precision: 22, scale: 8));
            AlterColumn("dbo.filters", "minNotional", c => c.Decimal(precision: 22, scale: 8));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.filters", "minNotional", c => c.Decimal(precision: 18, scale: 8));
            AlterColumn("dbo.filters", "stepSize", c => c.Decimal(precision: 18, scale: 8));
            AlterColumn("dbo.filters", "maxQty", c => c.Decimal(precision: 18, scale: 8));
            AlterColumn("dbo.filters", "minQty", c => c.Decimal(precision: 18, scale: 8));
            AlterColumn("dbo.filters", "multiplierDown", c => c.Decimal(precision: 18, scale: 8));
            AlterColumn("dbo.filters", "multiplierUp", c => c.Decimal(precision: 18, scale: 8));
            AlterColumn("dbo.filters", "tickSize", c => c.Decimal(precision: 18, scale: 8));
            AlterColumn("dbo.filters", "maxPrice", c => c.Decimal(precision: 18, scale: 8));
            AlterColumn("dbo.filters", "minPrice", c => c.Decimal(precision: 18, scale: 8));
        }
    }
}
