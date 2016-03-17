using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.NodaTime;
using HealthcareAnalytics.Utilities.CsvMapping;
using HealthcareAnalytics.Utilities.Entities;
using NodaTime;

namespace HealthcareAnalytics.Utilities
{
    public static class ExtractTransformLoadUtilities
    {
        private static string ConnectionString;

        static ExtractTransformLoadUtilities()
        {
            var connStrBuilder = new SqlConnectionStringBuilder
            {
                DataSource = @"localhost\MSSQL2016RC0",
                InitialCatalog = "HealthcareAnalytics",
                IntegratedSecurity = true
            };

            ConnectionString = connStrBuilder.ToString();

            DapperNodaTimeSetup.Register();
        }

        public static void LoadInpatientClaimsIntoDb(IEnumerable<InpatientClaim> inpatientClaims)
        {
            var insert = GenerateInsertSqlScript<InpatientClaim>();

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(insert, inpatientClaims);
            }
        }

        public static void LoadBeneficiarySummariesIntoDb(IEnumerable<BeneficiarySummary> summaries)
        {
            var insert = GenerateInsertSqlScript<BeneficiarySummary>();

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(insert, summaries);
            }
        }

        public static string GenerateCreateTableSqlScript<TEntity>()
        {
            var entity = typeof (TEntity);

            var attrMappings = new Dictionary<Type, string>
            {
                [typeof(string)] = "NVARCHAR(100)",
                [typeof(LocalDate?)] = "DATE",
                [typeof(decimal?)] = "DECIMAL(18,0)",
                [typeof(int?)] = "INT",
                [typeof(int)] = "INT",
                [typeof(bool)] = "BIT"
            };

            var props = entity.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"CREATE TABLE [{entity.Name}] (");

            foreach (var prop in props)
            {
                if (attrMappings.ContainsKey(prop.PropertyType))
                {
                    stringBuilder.Append($"[{prop.Name}] {attrMappings[prop.PropertyType]},");
                }

                if (prop.PropertyType.IsEnum)
                {
                    stringBuilder.Append($"[{prop.Name}] INT,");
                }
            }

            if (props.Any())
            {
                stringBuilder.Length--;
            }

            stringBuilder.Append(")");

            return stringBuilder.ToString();
        }

        public static string GenerateInsertSqlScript<TEntity>()
        {
            var entity = typeof (TEntity);
            var props = entity.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"INSERT INTO [{entity.Name}] (");

            var colNames = props
                .Where(p => p.PropertyType != typeof (IEnumerable<string>))
                .Select(p => p.Name)
                .ToList();

            for (var index = 0; index < colNames.Count; index++)
            {
                var name = colNames[index];
                stringBuilder.Append($"\t{name}");

                if (index + 1 != colNames.Count)
                {
                    stringBuilder.Append(",");
                }

                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine(")");
            stringBuilder.AppendLine("VALUES (");

            for (var index = 0; index < colNames.Count; index++)
            {
                var name = colNames[index];
                stringBuilder.Append($"\t@{name}");

                if (index + 1 != colNames.Count)
                {
                    stringBuilder.Append(",");
                }

                stringBuilder.AppendLine();
            }

            stringBuilder.Append(")");

            return stringBuilder.ToString();
        }


    }
}
