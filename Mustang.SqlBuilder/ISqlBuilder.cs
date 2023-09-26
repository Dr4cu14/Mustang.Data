using System.Collections.Generic;

namespace Mustang.SqlBuilder;

public interface ISqlBuilder
{
    public string Sql { get; set; }

    public List<SqlParameter> SqlParameters { get; set; }
}