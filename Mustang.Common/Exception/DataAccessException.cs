using System.Data.Common;
using System.Text;

namespace Mustang.Common;

public class DataAccessException : ApplicationException
{

    public DataAccessException(
        Exception ex,
        string connectionStr,
        string sqlText,
        params DbParameterCollection[] parameters) : base(BuilderMessage(ex.Message, connectionStr, sqlText, parameters), ex)
    {

    }

    private static string BuilderMessage(string message, string connectionStr, string sql, params DbParameterCollection[] parameters)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"{message}");
        stringBuilder.AppendLine($"Connection String: {connectionStr}");
        stringBuilder.AppendLine($"SQL Script : {sql}");
        if (parameters.Any() && parameters.Length > 0)
        {
            stringBuilder.AppendLine("SQL Parameter(s): ");
            foreach (DbParameterCollection parameterCollection in parameters)
            {
                for (int i = 0; i < parameterCollection.Count; i++)
                {
                    var dbParameter = parameterCollection[0];
                    var valueType = dbParameter.Value is null ? "" : dbParameter.Value.GetType().ToString();
                    stringBuilder.Append($"{dbParameter.ParameterName} [{dbParameter.DbType}] : {dbParameter.Value} [{valueType}]");
                }
            }
        }

        return stringBuilder.ToString();
    }

}