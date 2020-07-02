namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _014 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RecuperarSenha",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token = c.String(nullable: false, maxLength: 100, unicode: false),
                        DataCadastro = c.DateTimeOffset(nullable: false, precision: 7),
                        Usuario_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.Usuario_Id)
                .Index(t => t.Usuario_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecuperarSenha", "Usuario_Id", "dbo.Usuario");
            DropIndex("dbo.RecuperarSenha", new[] { "Usuario_Id" });
            DropTable("dbo.RecuperarSenha");
        }
    }
}
