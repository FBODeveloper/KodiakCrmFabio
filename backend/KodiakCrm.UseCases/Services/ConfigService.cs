using KodiakCrm.Core.DTOs;
using KodiakCrm.Core.Entities;
using KodiakCrm.Core.Interfaces;

namespace KodiakCrm.UseCases.Services;

public class ConfigService
{
    private readonly IConfigRepository _repository;

    public ConfigService(IConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmpresaConfigDTO?> ObterAsync(string cnpjEmpresa)
    {
        var config = await _repository.ObterPorCnpjAsync(cnpjEmpresa);
        if (config == null)
        {
            config = new EmpresaConfig { CnpjEmpresa = cnpjEmpresa };
            await _repository.CriarAsync(config);
            return MapearParaDTO(config);
        }

        return MapearParaDTO(config);
    }

    public async Task<(bool sucesso, string mensagem)> AtualizarAsync(string cnpjEmpresa, EmpresaConfigUpdateDTO dto)
    {
        var config = await _repository.ObterPorCnpjAsync(cnpjEmpresa);
        if (config == null)
        {
            config = new EmpresaConfig { CnpjEmpresa = cnpjEmpresa };
            await _repository.CriarAsync(config);
        }

        if (dto.Tema != null) config!.Tema = dto.Tema;
        if (dto.FusoHorario != null) config!.FusoHorario = dto.FusoHorario;
        if (dto.Moeda != null) config!.Moeda = dto.Moeda;
        if (dto.Idioma != null) config!.Idioma = dto.Idioma;
        if (dto.NotificacoesEmail.HasValue) config!.NotificacoesEmail = dto.NotificacoesEmail.Value;
        if (dto.NotificacoesSistema.HasValue) config!.NotificacoesSistema = dto.NotificacoesSistema.Value;

        config!.DataAtualizacao = DateTime.UtcNow;

        await _repository.AtualizarAsync(config);

        return (true, "Configurações atualizadas com sucesso");
    }

    private static EmpresaConfigDTO MapearParaDTO(EmpresaConfig config)
    {
        return new EmpresaConfigDTO
        {
            Id = config.Id,
            CnpjEmpresa = config.CnpjEmpresa,
            Tema = config.Tema,
            FusoHorario = config.FusoHorario,
            Moeda = config.Moeda,
            Idioma = config.Idioma,
            NotificacoesEmail = config.NotificacoesEmail,
            NotificacoesSistema = config.NotificacoesSistema
        };
    }
}
