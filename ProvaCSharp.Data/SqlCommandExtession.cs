using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaCSharp.Data
{
    public static class SqlCommandExtession
    {
        public static string GetGeneratedQuery(this SqlCommand dbCommand)
        {
            var query = dbCommand.CommandText;
            foreach (SqlParameter parameter in dbCommand.Parameters)
            {
                if (parameter.DbType == System.Data.DbType.String || parameter.DbType == System.Data.DbType.DateTime)
                    query = query.Replace(parameter.ParameterName, $"'{parameter.Value.ToString()}'");
                else
                    query = query.Replace(parameter.ParameterName, parameter.Value.ToString());
            }

            return query;
        }

        public static void AdicionaParametroValor(this SqlCommand dbCommand, string parameterString, DbType type, object value)
        {
            var parameter = new SqlParameter(parameterString, type);
            parameter.Value =value;
            dbCommand.Parameters.Add(parameter);
        }

    }
}
