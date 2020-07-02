namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _004 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Ordem", new[] { "chamada_Id" });
            DropIndex("dbo.EdicaoAceita", new[] { "chamadaEditada_ID" });
            DropIndex("dbo.EdicaoAceita", new[] { "tipoEdicao_ID" });
            CreateIndex("dbo.Ordem", "Chamada_Id");
            CreateIndex("dbo.EdicaoAceita", "ChamadaEditada_ID");
            CreateIndex("dbo.EdicaoAceita", "TipoEdicao_ID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.EdicaoAceita", new[] { "TipoEdicao_ID" });
            DropIndex("dbo.EdicaoAceita", new[] { "ChamadaEditada_ID" });
            DropIndex("dbo.Ordem", new[] { "Chamada_Id" });
            CreateIndex("dbo.EdicaoAceita", "tipoEdicao_ID");
            CreateIndex("dbo.EdicaoAceita", "chamadaEditada_ID");
            CreateIndex("dbo.Ordem", "chamada_Id");
        }
    }
}
