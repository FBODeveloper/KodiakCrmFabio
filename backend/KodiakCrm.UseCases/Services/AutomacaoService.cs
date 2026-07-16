using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class AutomacaoService
{
    private readonly IAtividadeRepository _atividadeRepository;
    private readonly INotificacaoRepository _notificacaoRepository;

    public AutomacaoService(IAtividadeRepository atividadeRepository, INotificacaoRepository notificacaoRepository)
    {
        _atividadeRepository = atividadeRepository;
        _notificacaoRepository = notificacaoRepository;
    }

    public async Task CriarFollowUpLeadAsync(string idEmpresa, int leadId, string leadNome, int? responsavelId, string responsavelNome)
    {
        if (!responsavelId.HasValue) return;

        var followUp = new Atividade
        {
            IdEmpresa = idEmpresa,
            Tipo = "followup",
            Titulo = $"Follow-up: {leadNome}",
            Descricao = $"Atividade automática de follow-up criada para o lead {leadNome}",
            ResponsavelId = responsavelId.Value,
            DataInicio = DateTime.UtcNow.AddDays(3),
            DataFim = DateTime.UtcNow.AddDays(3).AddHours(1),
            Concluida = false
        };

        var id = await _atividadeRepository.CriarAsync(followUp);

        await _notificacaoRepository.CriarAsync(new Notificacao
        {
            IdEmpresa = idEmpresa,
            UsuarioId = responsavelId.Value,
            Titulo = "Follow-up automático criado",
            Mensagem = $"Um follow-up foi agendado para o lead \"{leadNome}\" em 3 dias",
            Tipo = "followup",
            Entidade = "lead",
            EntidadeId = leadId
        });
    }

    public async Task CriarFollowUpOportunidadeAsync(string idEmpresa, int oportunidadeId, string oportunidadeTitulo, int? responsavelId)
    {
        if (!responsavelId.HasValue) return;

        var followUp = new Atividade
        {
            IdEmpresa = idEmpresa,
            Tipo = "followup",
            Titulo = $"Follow-up: {oportunidadeTitulo}",
            Descricao = $"Atividade automática de follow-up para a oportunidade {oportunidadeTitulo}",
            ResponsavelId = responsavelId.Value,
            DataInicio = DateTime.UtcNow.AddDays(5),
            DataFim = DateTime.UtcNow.AddDays(5).AddHours(1),
            Concluida = false
        };

        await _atividadeRepository.CriarAsync(followUp);
    }

    public async Task VerificarAtividadesAtrasadasAsync()
    {
        var empresas = new List<string>();
        var now = DateTime.UtcNow;

        var atividadesAtrasadas = await _atividadeRepository.ObterAtividadesAtrasadasAsync(now);

        foreach (var atividade in atividadesAtrasadas)
        {
            if (!atividade.ResponsavelId.HasValue) continue;

            var notificacao = new Notificacao
            {
                IdEmpresa = atividade.IdEmpresa,
                UsuarioId = atividade.ResponsavelId.Value,
                Titulo = "Atividade atrasada",
                Mensagem = $"A atividade \"{atividade.Titulo}\" está atrasada depuis {Math.Abs((now - (atividade.DataFim ?? now)).Days)} dia(s)",
                Tipo = "atividade",
                Entidade = "atividade",
                EntidadeId = atividade.Id
            };

            await _notificacaoRepository.CriarAsync(notificacao);
        }
    }
}
