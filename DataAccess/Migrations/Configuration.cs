namespace DataAccess.Migrations
{
    using DataAccess.Entidades;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccess.Context.CryptoContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DataAccess.Context.CryptoContext context)
        {
            var ChamadaStatus = new List<ChamadaStatus>();
            ChamadaStatus.Add(new ChamadaStatus { Id = 1, Descricao = "Aguardando Entrada" });
            ChamadaStatus.Add(new ChamadaStatus { Id = 2, Descricao = "Aguardando Loss/Gain" });
            ChamadaStatus.Add(new ChamadaStatus { Id = 3, Descricao = "Cancelada" });
            ChamadaStatus.Add(new ChamadaStatus { Id = 4, Descricao = "Finalizada" });
            ChamadaStatus.ForEach(item => context.chamadastatus.AddOrUpdate(x => x.Descricao, item));

            var OrdemStatus = new List<OrdemStatus>();
            OrdemStatus.Add(new OrdemStatus { Id = 1, Descricao = "Aguardando Entrada" });
            OrdemStatus.Add(new OrdemStatus { Id = 2, Descricao = "Vendida a Mercado" });
            OrdemStatus.Add(new OrdemStatus { Id = 3, Descricao = "Entrada Realizada" });
            OrdemStatus.Add(new OrdemStatus { Id = 4, Descricao = "Ordem Cancelada" });
            OrdemStatus.Add(new OrdemStatus { Id = 5, Descricao = "Executada com Gain" });
            OrdemStatus.Add(new OrdemStatus { Id = 6, Descricao = "Executada com Loss" });
            OrdemStatus.Add(new OrdemStatus { Id = 7, Descricao = "Ordem Rejeitada" });
            OrdemStatus.ForEach(item => context.ordemStatus.AddOrUpdate(x => x.Descricao, item));

            var BinanceStatus = new List<BinanceStatus>();
            BinanceStatus.Add(new BinanceStatus { Id = 1, Descricao = "NEW" });
            BinanceStatus.Add(new BinanceStatus { Id = 2, Descricao = "PARTIALLY_FILLED" });
            BinanceStatus.Add(new BinanceStatus { Id = 3, Descricao = "FILLED" });
            BinanceStatus.Add(new BinanceStatus { Id = 4, Descricao = "CANCELED" });
            BinanceStatus.Add(new BinanceStatus { Id = 5, Descricao = "PENDING_CANCEL" });
            BinanceStatus.Add(new BinanceStatus { Id = 6, Descricao = "REJECTED" });
            BinanceStatus.Add(new BinanceStatus { Id = 7, Descricao = "EXPIRED" });
            BinanceStatus.ForEach(item => context.binanceStatus.AddOrUpdate(x => x.Descricao, item));

            var TipoOrdem = new List<TipoOrdem>();
            TipoOrdem.Add(new TipoOrdem { Id = 1, Descricao = "Entrada" });
            TipoOrdem.Add(new TipoOrdem { Id = 2, Descricao = "Oco" });
            TipoOrdem.Add(new TipoOrdem { Id = 3, Descricao = "Venda Mercado" });
            TipoOrdem.ForEach(item => context.tipoOrdem.AddOrUpdate(x => x.Descricao, item));

            var TipoEdicaoAceita = new List<TipoEdicaoAceita>();
            TipoEdicaoAceita.Add(new TipoEdicaoAceita { Id = 1, Descricao = "Aceita" });
            TipoEdicaoAceita.Add(new TipoEdicaoAceita { Id = 2, Descricao = "Recusada" });
            TipoEdicaoAceita.ForEach(item => context.tipoEdicaoAceita.AddOrUpdate(x => x.Descricao, item));

            var MetodoPagamento = new List<MetodoPagamento>();
            MetodoPagamento.Add(new MetodoPagamento { Id = 1, Descricao = "Pag Seguro" });
            MetodoPagamento.Add(new MetodoPagamento { Id = 2, Descricao = "BitPay" });
            MetodoPagamento.ForEach(item => context.metodoPagamento.AddOrUpdate(x => x.Descricao, item));

            var PagamentoLicencaStatus = new List<PagamentoLicencaStatus>();
            PagamentoLicencaStatus.Add(new PagamentoLicencaStatus { Id = 1, Descricao = "Aguardando Pagamento" });
            PagamentoLicencaStatus.Add(new PagamentoLicencaStatus { Id = 2, Descricao = "Pagamento Rejeitado" });
            PagamentoLicencaStatus.Add(new PagamentoLicencaStatus { Id = 3, Descricao = "Pagamento Confirmado" });
            PagamentoLicencaStatus.ForEach(item => context.pagamentoLicencastatus.AddOrUpdate(x => x.Descricao, item));

            var ResultadoChamada = new List<ResultadoChamada>();
            ResultadoChamada.Add(new ResultadoChamada { Id = 1, Descricao = "Gain" });
            ResultadoChamada.Add(new ResultadoChamada { Id = 2, Descricao = "Loss" });
            ResultadoChamada.ForEach(item => context.resultadoChamada.AddOrUpdate(x => x.Descricao, item));

            var MotivoCancelamento = new List<MotivoCancelamento>();
            MotivoCancelamento.Add(new MotivoCancelamento { Id = 1, Descricao = "Entrada" });
            MotivoCancelamento.Add(new MotivoCancelamento { Id = 2, Descricao = "Venda Mercado" });
            MotivoCancelamento.Add(new MotivoCancelamento { Id = 3, Descricao = "Edicao Aceita" });
            MotivoCancelamento.Add(new MotivoCancelamento { Id = 4, Descricao = "Cancelamento Oco Painel Binance" });
            MotivoCancelamento.ForEach(item => context.motivoCancelamento.AddOrUpdate(x => x.Descricao, item));

            var Analista = new Analista { DataCadastro = DateTime.Now, Email = "admin@admin.com.br", Ativo = true, Nome = "Doing Now", Sobrenome = "", Password = "$2a$10$x414cgP/IdRGYsSU6gpYK.dgF80ikvwLPQkYUWrrNw6VleO.ps5dC" };
            context.analista.AddOrUpdate(x => x.Email, Analista);          

            context.SaveChanges();
        }
    }
}
