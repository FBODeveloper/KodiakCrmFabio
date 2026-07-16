namespace KodiakCrm.Core.Entities;

public class Notificacao
{
    public int Id { get; set; }
    public string IdEmpresa { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public string Tipo { get; set; } = "sistema";
    public string? Entidade { get; set; }
    public int? EntidadeId { get; set; }
    public bool Lida { get; set; } = false;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataLeitura { get; set; }
}
