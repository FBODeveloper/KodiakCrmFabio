using System.Text.Json;
using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class RelatorioService
{
    private readonly IRelatorioRepository _repository;

    public RelatorioService(IRelatorioRepository repository)
    {
        _repository = repository;
    }

    public async Task<RelatorioVendasDTO> GerarRelatorioVendasAsync(string idEmpresa, string cnpjEmpresa, RelatorioFiltroDTO filtro, int? usuarioId, string? usuarioNome)
    {
        var oportunidades = await _repository.ObterOportunidadesAsync(idEmpresa, filtro.DataInicio, filtro.DataFim, filtro.Status, filtro.ResponsavelId);

        var ganhas = oportunidades.Where(o => o.Status == "ganha").ToList();
        var perdidas = oportunidades.Where(o => o.Status == "perdida").ToList();
        var abertas = oportunidades.Where(o => o.Status == "aberta").ToList();

        var valorTotal = oportunidades.Where(o => o.Valor.HasValue).Sum(o => o.Valor!.Value);
        var valorGanho = ganhas.Where(o => o.Valor.HasValue).Sum(o => o.Valor!.Value);
        var valorPerdido = perdidas.Where(o => o.Valor.HasValue).Sum(o => o.Valor!.Value);
        var totalComValor = oportunidades.Count(o => o.Valor.HasValue);

        var porPeriodo = oportunidades
            .GroupBy(o => o.DataCadastro.ToString("yyyy-MM"))
            .OrderBy(g => g.Key)
            .Select(g => new RelatorioVendasPorPeriodoDTO
            {
                Periodo = g.Key,
                Quantidade = g.Count(),
                Valor = g.Where(o => o.Valor.HasValue).Sum(o => o.Valor!.Value)
            }).ToList();

        var porResponsavel = oportunidades
            .GroupBy(o => o.ResponsavelNome ?? "Sem responsável")
            .Select(g => new RelatorioVendasPorResponsavelDTO
            {
                ResponsavelNome = g.Key,
                Total = g.Count(),
                Ganhas = g.Count(o => o.Status == "ganha"),
                ValorTotal = g.Where(o => o.Valor.HasValue).Sum(o => o.Valor!.Value)
            }).OrderByDescending(r => r.ValorTotal).ToList();

        var resultado = new RelatorioVendasDTO
        {
            TotalOportunidades = oportunidades.Count,
            OportunidadesGanhas = ganhas.Count,
            OportunidadesPerdidas = perdidas.Count,
            OportunidadesAbertas = abertas.Count,
            ValorTotal = valorTotal,
            ValorGanho = valorGanho,
            ValorPerdido = valorPerdido,
            TicketMedio = totalComValor > 0 ? valorTotal / totalComValor : 0,
            TaxaConversao = oportunidades.Count > 0 ? (decimal)ganhas.Count / oportunidades.Count * 100 : 0,
            PorPeriodo = porPeriodo,
            PorResponsavel = porResponsavel
        };

        await SalvarRelatorioAsync(idEmpresa, cnpjEmpresa, "vendas", "Relatório de Vendas", filtro, resultado, usuarioId, usuarioNome);

        return resultado;
    }

    public async Task<RelatorioAtividadesDTO> GerarRelatorioAtividadesAsync(string idEmpresa, string cnpjEmpresa, RelatorioFiltroDTO filtro, int? usuarioId, string? usuarioNome)
    {
        var atividades = await _repository.ObterAtividadesAsync(idEmpresa, filtro.DataInicio, filtro.DataFim, filtro.TipoAtividade, filtro.ResponsavelId);

        var concluidas = atividades.Count(a => a.Concluida);
        var pendentes = atividades.Count(a => !a.Concluida);

        var porTipo = atividades
            .GroupBy(a => a.Tipo)
            .Select(g => new RelatorioAtividadesPorTipoDTO
            {
                Tipo = g.Key,
                Quantidade = g.Count(),
                Concluidas = g.Count(a => a.Concluida)
            }).OrderByDescending(t => t.Quantidade).ToList();

        var porResponsavel = atividades
            .GroupBy(a => a.ResponsavelNome ?? "Sem responsável")
            .Select(g => new RelatorioAtividadesPorResponsavelDTO
            {
                ResponsavelNome = g.Key,
                Total = g.Count(),
                Concluidas = g.Count(a => a.Concluida)
            }).OrderByDescending(r => r.Total).ToList();

        var resultado = new RelatorioAtividadesDTO
        {
            TotalAtividades = atividades.Count,
            Concluidas = concluidas,
            Pendentes = pendentes,
            TaxaConclusao = atividades.Count > 0 ? (decimal)concluidas / atividades.Count * 100 : 0,
            PorTipo = porTipo,
            PorResponsavel = porResponsavel
        };

        await SalvarRelatorioAsync(idEmpresa, cnpjEmpresa, "atividades", "Relatório de Atividades", filtro, resultado, usuarioId, usuarioNome);

        return resultado;
    }

    public async Task<RelatorioPerformanceDTO> GerarRelatorioPerformanceAsync(string idEmpresa, string cnpjEmpresa, RelatorioFiltroDTO filtro, int? usuarioId, string? usuarioNome)
    {
        var leads = await _repository.ObterLeadsAsync(idEmpresa, filtro.DataInicio, filtro.DataFim, filtro.ResponsavelId);
        var oportunidades = await _repository.ObterOportunidadesAsync(idEmpresa, filtro.DataInicio, filtro.DataFim, null, filtro.ResponsavelId);
        var atividades = await _repository.ObterAtividadesAsync(idEmpresa, filtro.DataInicio, filtro.DataFim, null, filtro.ResponsavelId);

        var todosVendedores = leads.Select(l => new { l.ResponsavelId, l.ResponsavelNome })
            .Union(oportunidades.Select(o => new { o.ResponsavelId, o.ResponsavelNome }))
            .Union(atividades.Select(a => new { a.ResponsavelId, a.ResponsavelNome }))
            .Where(v => v.ResponsavelId.HasValue)
            .GroupBy(v => v.ResponsavelId!.Value)
            .Select(g => new RelatorioPerformanceVendedorDTO
            {
                VendedorNome = g.First().ResponsavelNome ?? "Desconhecido",
                TotalLeads = leads.Count(l => l.ResponsavelId == g.Key),
                TotalOportunidades = oportunidades.Count(o => o.ResponsavelId == g.Key),
                OportunidadesGanhas = oportunidades.Count(o => o.ResponsavelId == g.Key && o.Status == "ganha"),
                ValorTotal = oportunidades.Where(o => o.ResponsavelId == g.Key && o.Valor.HasValue).Sum(o => o.Valor!.Value),
                TaxaConversao = oportunidades.Count(o => o.ResponsavelId == g.Key) > 0
                    ? (decimal)oportunidades.Count(o => o.ResponsavelId == g.Key && o.Status == "ganha") / oportunidades.Count(o => o.ResponsavelId == g.Key) * 100
                    : 0,
                TotalAtividades = atividades.Count(a => a.ResponsavelId == g.Key)
            }).OrderByDescending(v => v.ValorTotal).ToList();

        var resultado = new RelatorioPerformanceDTO { Vendedores = todosVendedores };

        await SalvarRelatorioAsync(idEmpresa, cnpjEmpresa, "performance", "Relatório de Performance", filtro, resultado, usuarioId, usuarioNome);

        return resultado;
    }

    public async Task<List<RelatorioGeradoDTO>> ObterRecentesAsync(string idEmpresa, int limite = 20)
    {
        var relatorios = await _repository.ObterRecentesAsync(idEmpresa, limite);
        return relatorios.Select(r => new RelatorioGeradoDTO
        {
            Id = r.Id,
            Tipo = r.Tipo,
            Titulo = r.Titulo,
            Parametros = r.Parametros,
            Resultado = r.Resultado,
            UsuarioNome = r.UsuarioNome,
            DataGeracao = r.DataGeracao
        }).ToList();
    }

    private async Task SalvarRelatorioAsync(string idEmpresa, string cnpjEmpresa, string tipo, string titulo, RelatorioFiltroDTO filtro, object resultado, int? usuarioId, string? usuarioNome)
    {
        var parametros = JsonSerializer.Serialize(filtro);
        var resultadoJson = JsonSerializer.Serialize(resultado);

        var relatorio = new RelatorioGerado
        {
            IdEmpresa = idEmpresa,
            CnpjEmpresa = cnpjEmpresa,
            Tipo = tipo,
            Titulo = titulo,
            Parametros = parametros,
            Resultado = resultadoJson,
            UsuarioId = usuarioId,
            UsuarioNome = usuarioNome
        };

        await _repository.CriarAsync(relatorio);
    }
}
