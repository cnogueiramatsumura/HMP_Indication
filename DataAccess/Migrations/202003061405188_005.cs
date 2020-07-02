namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _005 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EdicaoAceita", "Chamada_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.EdicaoAceita", "Chamada_ID");
            AddForeignKey("dbo.EdicaoAceita", "Chamada_ID", "dbo.Chamada", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EdicaoAceita", "Chamada_ID", "dbo.Chamada");
            DropIndex("dbo.EdicaoAceita", new[] { "Chamada_ID" });
            DropColumn("dbo.EdicaoAceita", "Chamada_ID");
        }
    }
}
