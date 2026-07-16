using KodiakCrm.Core.Entities;

namespace KodiakCrm.Core.Entities;

public class Usuario : BaseEntity
{
    public int? IdUsuarioKodiak { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public DateOnly? DataNascimento { get; set; }
    public string Perfil { get; set; } = "usuario";
}
