namespace KodiakCrm.Core.DTOs;

public class NotificacaoDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string? Entidade { get; set; }
    public int? EntidadeId { get; set; }
    public bool Lida { get; set; }
    public DateTime DataCriacao { get; set; }
}

public class NotificacaoCriarDTO
{
    public int UsuarioId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public string Tipo { get; set; } = "sistema";
    public string? Entidade { get; set; }
    public int? EntidadeId { get; set; }
}

public class NotificacaoResumoDTO
{
    public int TotalNaoLidas { get; set; }
    public List<NotificacaoDTO> Recentes { get; set; } = new();
}
