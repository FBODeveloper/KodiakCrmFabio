namespace KodiakCrm.Core.Entities;

public class LeadEstagio
{
    public int Id { get; set; }
    public string IdEmpresa { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public string Cor { get; set; } = "#3b82f6";
}
