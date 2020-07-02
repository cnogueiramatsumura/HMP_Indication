namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _012 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MotivoCancelamento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Ordem", "MotivoCancelamento_ID", c => c.Int());
            CreateIndex("dbo.Ordem", "MotivoCancelamento_ID");
            AddForeignKey("dbo.Ordem", "MotivoCancelamento_ID", "dbo.MotivoCancelamento", "Id");
            DropColumn("dbo.Ordem", "VendidaMercado");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ordem", "VendidaMercado", c => c.Boolean());
            DropForeignKey("dbo.Ordem", "MotivoCancelamento_ID", "dbo.MotivoCancelamento");
            DropIndex("dbo.Ordem", new[] { "MotivoCancelamento_ID" });
            DropColumn("dbo.Ordem", "MotivoCancelamento_ID");
            DropTable("dbo.MotivoCancelamento");
        }
    }
}
