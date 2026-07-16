namespace KodiakCrm.Core.Entities;

public class EmpresaConfig
{
    public int Id { get; set; }
    public string CnpjEmpresa { get; set; } = string.Empty;
    public string Tema { get; set; } = "light";
    public string FusoHorario { get; set; } = "America/Sao_Paulo";
    public string Moeda { get; set; } = "BRL";
    public string Idioma { get; set; } = "pt-BR";
    public bool NotificacoesEmail { get; set; } = true;
    public bool NotificacoesSistema { get; set; } = true;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
}
