using System.Data;

namespace KodiakCrm.Core.Interfaces;

public interface IDatabaseConnection
{
    IDbConnection GetConnection();
}
