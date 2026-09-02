using System.Data;
using Dapper;

namespace Healtive.Infrastructure.Data;

public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(
        IDbDataParameter parameter,
        DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }

    public override DateOnly Parse(object value)
    {
        if (value is DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime);
        }

        if (value is DateOnly dateOnly)
        {
            return dateOnly;
        }

        return DateOnly.Parse(value.ToString()!);
    }
}