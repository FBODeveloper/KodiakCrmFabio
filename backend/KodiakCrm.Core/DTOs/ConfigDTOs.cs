namespace KodiakCrm.Core.DTOs;

public class EmpresaConfigDTO
{
    public int Id { get; set; }
    public string CnpjEmpresa { get; set; } = string.Empty;
    public string Tema { get; set; } = "light";
    public string FusoHorario { get; set; } = "America/Sao_Paulo";
    public string Moeda { get; set; } = "BRL";
    public string Idioma { get; set; } = "pt-BR";
    public bool NotificacoesEmail { get; set; } = true;
    public bool NotificacoesSistema { get; set; } = true;
}

public class EmpresaConfigUpdateDTO
{
    public string? Tema { get; set; }
    public string? FusoHorario { get; set; }
    public string? Moeda { get; set; }
    public string? Idioma { get; set; }
    public bool? NotificacoesEmail { get; set; }
    public bool? NotificacoesSistema { get; set; }
}
