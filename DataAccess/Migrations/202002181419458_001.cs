namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Analista",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 150, unicode: false),
                        DataCadastro = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BinanceStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ordem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataCadastro = c.DateTimeOffset(nullable: false, precision: 7),
                        Quantidade = c.Decimal(nullable: false, precision: 18, scale: 8),
                        OrderID = c.String(maxLength: 100, unicode: false),
                        OcoOrderListId = c.String(maxLength: 100, unicode: false),
                        LimitOrder_ID = c.String(maxLength: 10, unicode: false),
                        StopOrder_ID = c.String(maxLength: 10, unicode: false),
                        DataExecucao = c.DateTimeOffset(precision: 7),
                        DataCancelamento = c.DateTimeOffset(precision: 7),
                        chamada_Id = c.Int(nullable: false),
                        Usuario_Id = c.Int(nullable: false),
                        OrdemStatus_Id = c.Int(nullable: false),
                        BinanceStatus_Id = c.Int(),
                        TipoOrdem_Id = c.Int(nullable: false),
                        MainOrderID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.Usuario_Id)
                .ForeignKey("dbo.Chamada", t => t.chamada_Id)
                .ForeignKey("dbo.OrdemStatus", t => t.OrdemStatus_Id)
                .ForeignKey("dbo.TipoOrdem", t => t.TipoOrdem_Id)
                .ForeignKey("dbo.BinanceStatus", t => t.BinanceStatus_Id)
                .Index(t => t.chamada_Id)
                .Index(t => t.Usuario_Id)
                .Index(t => t.OrdemStatus_Id)
                .Index(t => t.BinanceStatus_Id)
                .Index(t => t.TipoOrdem_Id);
            
            CreateTable(
                "dbo.Chamada",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataCadastro = c.DateTimeOffset(nullable: false, precision: 7),
                        PrecoMercadoHoraChamada = c.Decimal(nullable: false, precision: 18, scale: 8),
                        PrecoEntrada = c.Decimal(nullable: false, precision: 18, scale: 8),
                        RangeEntrada = c.Decimal(nullable: false, precision: 18, scale: 8),
                        PrecoGain = c.Decimal(nullable: false, precision: 18, scale: 8),
                        PrecoLoss = c.Decimal(nullable: false, precision: 18, scale: 8),
                        ChamadaStatus_Id = c.Int(nullable: false),
                        Symbol_id = c.Int(nullable: false),
                        Observarcao = c.String(maxLength: 500, unicode: false),
                        PercentualIndicado = c.Int(),
                        ResultadoChamada_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChamadaStatus", t => t.ChamadaStatus_Id)
                .ForeignKey("dbo.ResultadoChamada", t => t.ResultadoChamada_Id)
                .ForeignKey("dbo.Symbol", t => t.Symbol_id)
                .Index(t => t.ChamadaStatus_Id)
                .Index(t => t.Symbol_id)
                .Index(t => t.ResultadoChamada_Id);
            
            CreateTable(
                "dbo.ChamadaEditada",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataEdicao = c.DateTimeOffset(nullable: false, precision: 7),
                        PrecoGain = c.Decimal(nullable: false, precision: 18, scale: 8),
                        PrecoLoss = c.Decimal(nullable: false, precision: 18, scale: 8),
                        Chamada_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chamada", t => t.Chamada_Id)
                .Index(t => t.Chamada_Id);
            
            CreateTable(
                "dbo.EdicaoAceita",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        chamadaEditada_ID = c.Int(nullable: false),
                        Usuario_Id = c.Int(nullable: false),
                        tipoEdicao_ID = c.Int(nullable: false),
                        DataCadastro = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipoEdicaoAceita", t => t.tipoEdicao_ID)
                .ForeignKey("dbo.Usuario", t => t.Usuario_Id)
                .ForeignKey("dbo.ChamadaEditada", t => t.chamadaEditada_ID)
                .Index(t => t.chamadaEditada_ID)
                .Index(t => t.Usuario_Id)
                .Index(t => t.tipoEdicao_ID);
            
            CreateTable(
                "dbo.TipoEdicaoAceita",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        email = c.String(nullable: false, maxLength: 100, unicode: false),
                        emailConfirmado = c.Boolean(nullable: false),
                        nome = c.String(nullable: false, maxLength: 100, unicode: false),
                        sobrenome = c.String(maxLength: 100, unicode: false),
                        dataCadastro = c.DateTimeOffset(nullable: false, precision: 7),
                        DataVencimentoLicenca = c.DateTimeOffset(nullable: false, precision: 7),
                        password = c.String(nullable: false, maxLength: 128, unicode: false),
                        foto = c.String(maxLength: 100, unicode: false),
                        oneSignalIDWeb = c.String(maxLength: 100, unicode: false),
                        oneSignalIDApp = c.String(maxLength: 100, unicode: false),
                        AnalistaID = c.Int(),
                        BinanceAPIKey = c.String(maxLength: 100, unicode: false),
                        BinanceAPISecret = c.String(maxLength: 100, unicode: false),
                        isValidBinanceKeys = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChamadasRecusadas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Usuario_ID = c.Int(nullable: false),
                        Chamada_ID = c.Int(nullable: false),
                        HoraRecusada = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chamada", t => t.Chamada_ID)
                .ForeignKey("dbo.Usuario", t => t.Usuario_ID)
                .Index(t => t.Usuario_ID)
                .Index(t => t.Chamada_ID);
            
            CreateTable(
                "dbo.ConfirmEmail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Usuario_Id = c.Int(nullable: false),
                        token = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.Usuario_Id)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.PagamentoLicenca",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataCriacaoInvoice = c.DateTimeOffset(nullable: false, precision: 7),
                        DataPagamento = c.DateTimeOffset(precision: 7),
                        MetodoPagamentoId = c.Int(nullable: false),
                        PagamentoLicencaStatusId = c.Int(nullable: false),
                        ValoraReceber = c.Decimal(nullable: false, precision: 18, scale: 8),
                        ValorPago = c.Decimal(precision: 18, scale: 8),
                        Usuario_Id = c.Int(nullable: false),
                        CodigoPagSeguro = c.String(maxLength: 100, unicode: false),
                        CodigoBitPay = c.String(maxLength: 100, unicode: false),
                        tokenPagamento = c.String(nullable: false, maxLength: 100, unicode: false),
                        Qtd_BTC_Pago = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MetodoPagamento", t => t.MetodoPagamentoId)
                .ForeignKey("dbo.PagamentoLicencaStatus", t => t.PagamentoLicencaStatusId)
                .ForeignKey("dbo.Usuario", t => t.Usuario_Id)
                .Index(t => t.MetodoPagamentoId)
                .Index(t => t.PagamentoLicencaStatusId)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.MetodoPagamento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PagamentoLicencaStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChamadaStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResultadoChamada",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Symbol",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        symbol = c.String(nullable: false, maxLength: 100, unicode: false),
                        status = c.String(nullable: false, maxLength: 100, unicode: false),
                        baseAsset = c.String(nullable: false, maxLength: 100, unicode: false),
                        baseAssetPrecision = c.Int(nullable: false),
                        quoteAsset = c.String(nullable: false, maxLength: 100, unicode: false),
                        quotePrecision = c.Int(nullable: false),
                        icebergAllowed = c.Boolean(nullable: false),
                        ocoAllowed = c.Boolean(nullable: false),
                        isSpotTradingAllowed = c.Boolean(nullable: false),
                        isMarginTradingAllowed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.filters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        filterType = c.String(maxLength: 100, unicode: false),
                        minPrice = c.Decimal(precision: 18, scale: 8),
                        maxPrice = c.Decimal(precision: 18, scale: 8),
                        tickSize = c.Decimal(precision: 18, scale: 8),
                        multiplierUp = c.Decimal(precision: 18, scale: 8),
                        multiplierDown = c.Decimal(precision: 18, scale: 8),
                        minQty = c.Decimal(precision: 18, scale: 8),
                        maxQty = c.Decimal(precision: 18, scale: 8),
                        stepSize = c.Decimal(precision: 18, scale: 8),
                        minNotional = c.Decimal(precision: 18, scale: 8),
                        applyToMarket = c.Boolean(),
                        avgPriceMins = c.Int(),
                        limit = c.Int(),
                        maxNumAlgoOrders = c.Int(),
                        maxNumIcebergOrders = c.Int(),
                        Symbol_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Symbol", t => t.Symbol_Id)
                .Index(t => t.Symbol_Id);
            
            CreateTable(
                "dbo.OrdemStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrdemComission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Order_Id = c.Int(nullable: false),
                        ComissionAsset = c.String(nullable: false, maxLength: 100, unicode: false),
                        ComissionAmount = c.Decimal(nullable: false, precision: 18, scale: 8),
                        ValorExecutado = c.Decimal(nullable: false, precision: 18, scale: 8),
                        QtdExecutada = c.Decimal(nullable: false, precision: 18, scale: 8),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ordem", t => t.Order_Id)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.TipoOrdem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServerConfig",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bitpayToken = c.String(nullable: false, maxLength: 100, unicode: false),
                        bitpayIdentity = c.String(nullable: false, maxLength: 256, unicode: false),
                        pagseguroToken = c.String(nullable: false, maxLength: 100, unicode: false),
                        OneSignalAppId = c.String(nullable: false, maxLength: 100, unicode: false),
                        OneSignalToken = c.String(nullable: false, maxLength: 100, unicode: false),
                        ApiServer = c.String(nullable: false, maxLength: 100, unicode: false),
                        AppServer = c.String(maxLength: 100, unicode: false),
                        precoLicenca = c.Decimal(nullable: false, precision: 5, scale: 2),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ordem", "BinanceStatus_Id", "dbo.BinanceStatus");
            DropForeignKey("dbo.Ordem", "TipoOrdem_Id", "dbo.TipoOrdem");
            DropForeignKey("dbo.OrdemComission", "Order_Id", "dbo.Ordem");
            DropForeignKey("dbo.Ordem", "OrdemStatus_Id", "dbo.OrdemStatus");
            DropForeignKey("dbo.filters", "Symbol_Id", "dbo.Symbol");
            DropForeignKey("dbo.Chamada", "Symbol_id", "dbo.Symbol");
            DropForeignKey("dbo.Chamada", "ResultadoChamada_Id", "dbo.ResultadoChamada");
            DropForeignKey("dbo.Ordem", "chamada_Id", "dbo.Chamada");
            DropForeignKey("dbo.Chamada", "ChamadaStatus_Id", "dbo.ChamadaStatus");
            DropForeignKey("dbo.ChamadaEditada", "Chamada_Id", "dbo.Chamada");
            DropForeignKey("dbo.EdicaoAceita", "chamadaEditada_ID", "dbo.ChamadaEditada");
            DropForeignKey("dbo.PagamentoLicenca", "Usuario_Id", "dbo.Usuario");
            DropForeignKey("dbo.PagamentoLicenca", "PagamentoLicencaStatusId", "dbo.PagamentoLicencaStatus");
            DropForeignKey("dbo.PagamentoLicenca", "MetodoPagamentoId", "dbo.MetodoPagamento");
            DropForeignKey("dbo.Ordem", "Usuario_Id", "dbo.Usuario");
            DropForeignKey("dbo.EdicaoAceita", "Usuario_Id", "dbo.Usuario");
            DropForeignKey("dbo.ConfirmEmail", "Usuario_Id", "dbo.Usuario");
            DropForeignKey("dbo.ChamadasRecusadas", "Usuario_ID", "dbo.Usuario");
            DropForeignKey("dbo.ChamadasRecusadas", "Chamada_ID", "dbo.Chamada");
            DropForeignKey("dbo.EdicaoAceita", "tipoEdicao_ID", "dbo.TipoEdicaoAceita");
            DropIndex("dbo.OrdemComission", new[] { "Order_Id" });
            DropIndex("dbo.filters", new[] { "Symbol_Id" });
            DropIndex("dbo.PagamentoLicenca", new[] { "Usuario_Id" });
            DropIndex("dbo.PagamentoLicenca", new[] { "PagamentoLicencaStatusId" });
            DropIndex("dbo.PagamentoLicenca", new[] { "MetodoPagamentoId" });
            DropIndex("dbo.ConfirmEmail", new[] { "Usuario_Id" });
            DropIndex("dbo.ChamadasRecusadas", new[] { "Chamada_ID" });
            DropIndex("dbo.ChamadasRecusadas", new[] { "Usuario_ID" });
            DropIndex("dbo.EdicaoAceita", new[] { "tipoEdicao_ID" });
            DropIndex("dbo.EdicaoAceita", new[] { "Usuario_Id" });
            DropIndex("dbo.EdicaoAceita", new[] { "chamadaEditada_ID" });
            DropIndex("dbo.ChamadaEditada", new[] { "Chamada_Id" });
            DropIndex("dbo.Chamada", new[] { "ResultadoChamada_Id" });
            DropIndex("dbo.Chamada", new[] { "Symbol_id" });
            DropIndex("dbo.Chamada", new[] { "ChamadaStatus_Id" });
            DropIndex("dbo.Ordem", new[] { "TipoOrdem_Id" });
            DropIndex("dbo.Ordem", new[] { "BinanceStatus_Id" });
            DropIndex("dbo.Ordem", new[] { "OrdemStatus_Id" });
            DropIndex("dbo.Ordem", new[] { "Usuario_Id" });
            DropIndex("dbo.Ordem", new[] { "chamada_Id" });
            DropTable("dbo.ServerConfig");
            DropTable("dbo.TipoOrdem");
            DropTable("dbo.OrdemComission");
            DropTable("dbo.OrdemStatus");
            DropTable("dbo.filters");
            DropTable("dbo.Symbol");
            DropTable("dbo.ResultadoChamada");
            DropTable("dbo.ChamadaStatus");
            DropTable("dbo.PagamentoLicencaStatus");
            DropTable("dbo.MetodoPagamento");
            DropTable("dbo.PagamentoLicenca");
            DropTable("dbo.ConfirmEmail");
            DropTable("dbo.ChamadasRecusadas");
            DropTable("dbo.Usuario");
            DropTable("dbo.TipoEdicaoAceita");
            DropTable("dbo.EdicaoAceita");
            DropTable("dbo.ChamadaEditada");
            DropTable("dbo.Chamada");
            DropTable("dbo.Ordem");
            DropTable("dbo.BinanceStatus");
            DropTable("dbo.Analista");
        }
    }
}
