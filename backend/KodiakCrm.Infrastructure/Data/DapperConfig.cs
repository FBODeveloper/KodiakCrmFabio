using System.Reflection;
using System.Text.RegularExpressions;
using Dapper;
using KodiakCrm.Core.Entities;

namespace KodiakCrm.Infrastructure.Data;

public static class DapperConfig
{
    public static void Configure()
    {
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new NullableDateOnlyTypeHandler());
        SqlMapper.SetTypeMap(typeof(Usuario), CreateMap<Usuario>());
        SqlMapper.SetTypeMap(typeof(Empresa), CreateMap<Empresa>());
        SqlMapper.SetTypeMap(typeof(Parceiro), CreateMap<Parceiro>());
        SqlMapper.SetTypeMap(typeof(Lead), CreateMap<Lead>());
        SqlMapper.SetTypeMap(typeof(Funil), CreateMap<Funil>());
        SqlMapper.SetTypeMap(typeof(FunilEstagio), CreateMap<FunilEstagio>());
        SqlMapper.SetTypeMap(typeof(Oportunidade), CreateMap<Oportunidade>());
        SqlMapper.SetTypeMap(typeof(Atividade), CreateMap<Atividade>());
        SqlMapper.SetTypeMap(typeof(Proposta), CreateMap<Proposta>());
        SqlMapper.SetTypeMap(typeof(PropostaItem), CreateMap<PropostaItem>());
        SqlMapper.SetTypeMap(typeof(Historico), CreateMap<Historico>());
        SqlMapper.SetTypeMap(typeof(Cliente), CreateMap<Cliente>());
        SqlMapper.SetTypeMap(typeof(Contato), CreateMap<Contato>());
        SqlMapper.SetTypeMap(typeof(EmpresaConfig), CreateMap<EmpresaConfig>());
        SqlMapper.SetTypeMap(typeof(RelatorioGerado), CreateMap<RelatorioGerado>());
        SqlMapper.SetTypeMap(typeof(Notificacao), CreateMap<Notificacao>());
    }

    private static CustomPropertyTypeMap CreateMap<T>() where T : class
    {
        return new CustomPropertyTypeMap(typeof(T), (type, columnName) =>
        {
            var pascalName = ToPascalCase(columnName);
            return type.GetProperty(pascalName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        });
    }

    private static string ToPascalCase(string snakeCase)
    {
        return string.Concat(
            snakeCase.Split('_')
                .Select(part => char.ToUpper(part[0]) + part[1..])
        );
    }
}

public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value)
    {
        return value switch
        {
            DateTime dt => DateOnly.FromDateTime(dt),
            DateOnly d => d,
            _ => throw new InvalidCastException($"Cannot convert {value.GetType()} to DateOnly")
        };
    }

    public override void SetValue(System.Data.IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }
}

public class NullableDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly?>
{
    public override DateOnly? Parse(object value)
    {
        return value switch
        {
            DateTime dt => DateOnly.FromDateTime(dt),
            DateOnly d => d,
            DBNull => null,
            null => null,
            _ => throw new InvalidCastException($"Cannot convert {value.GetType()} to DateOnly?")
        };
    }

    public override void SetValue(System.Data.IDbDataParameter parameter, DateOnly? value)
    {
        parameter.Value = value?.ToDateTime(TimeOnly.MinValue) ?? (object)DBNull.Value;
    }
}
