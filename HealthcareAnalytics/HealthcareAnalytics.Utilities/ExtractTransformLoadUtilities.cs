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
            const string sproc = @"
	            @BeneficiaryCode NVARCHAR(100),
	            @ClaimId NVARCHAR(100),
	            @ClaimsStartDate DATE,
	            @ClaimsEndDate DATE,
	            @ProviderNumber NVARCHAR(100),
	            @ClaimPaymentAmount DECIMAL(18,0),
	            @NchPrimaryPayerClaimPaidAmount DECIMAL(18,0),
	            @AttendingPhysicianNpi NVARCHAR(100),
	            @OperatingPhysicianNpi NVARCHAR(100),
	            @OtherPhysicianNpi NVARCHAR(100),
	            @InpatientAdmissionDate DATE,
	            @ClaimAdmittingDiagnosisCode NVARCHAR(100),
	            @ClaimPassThruPerDiemAmount DECIMAL(18,0),
	            @NchBeneficiaryInpatientDeductibleAmount DECIMAL(18,0),
	            @NchBeneficiaryPartACoinsuranceLiabilityAmount DECIMAL(18,0),
	            @NchBeneficiaryBloodDeductibleLiabilityAmount DECIMAL(18,0),
	            @ClaimUtilizationDayCount DECIMAL(18,0),
	            @InpatientDischargedDate DATE,
	            @ClaimDiagnosisRelatedGroupCode NVARCHAR(100),
	            @ClaimDiagnosisCodes Icd9Codes READONLY,
	            @ClaimProcedureCodes Icd9Codes READONLY,
	            @RevenueCenterHcfaCpcsCodes Icd9Codes READONLY
            ";

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(sproc, inpatientClaims);
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
