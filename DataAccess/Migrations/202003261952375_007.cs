namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _007 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CancelamentoChamada",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Chamada_Id = c.Int(nullable: false),
                        DataCancelamento = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chamada", t => t.Chamada_Id)
                .Index(t => t.Chamada_Id);
            
            CreateTable(
                "dbo.CancelamentoRecusado",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataCancelamento = c.DateTimeOffset(nullable: false, precision: 7),
                        CancelamentoChamada_Id = c.Int(nullable: false),
                        Usuario_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.Usuario_Id)
                .ForeignKey("dbo.CancelamentoChamada", t => t.CancelamentoChamada_Id)
                .Index(t => t.CancelamentoChamada_Id)
                .Index(t => t.Usuario_Id);
            
            AddColumn("dbo.Chamada", "CancelamentoChamada_Id", c => c.Int());
            CreateIndex("dbo.Chamada", "CancelamentoChamada_Id");
            AddForeignKey("dbo.Chamada", "CancelamentoChamada_Id", "dbo.CancelamentoChamada", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chamada", "CancelamentoChamada_Id", "dbo.CancelamentoChamada");
            DropForeignKey("dbo.CancelamentoChamada", "Chamada_Id", "dbo.Chamada");
            DropForeignKey("dbo.CancelamentoRecusado", "CancelamentoChamada_Id", "dbo.CancelamentoChamada");
            DropForeignKey("dbo.CancelamentoRecusado", "Usuario_Id", "dbo.Usuario");
            DropIndex("dbo.CancelamentoRecusado", new[] { "Usuario_Id" });
            DropIndex("dbo.CancelamentoRecusado", new[] { "CancelamentoChamada_Id" });
            DropIndex("dbo.CancelamentoChamada", new[] { "Chamada_Id" });
            DropIndex("dbo.Chamada", new[] { "CancelamentoChamada_Id" });
            DropColumn("dbo.Chamada", "CancelamentoChamada_Id");
            DropTable("dbo.CancelamentoRecusado");
            DropTable("dbo.CancelamentoChamada");
        }
    }
}
