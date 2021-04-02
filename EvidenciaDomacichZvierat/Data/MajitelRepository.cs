using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using EvidenciaDomacichZvierat.Data.Helpers;
using EvidenciaDomacichZvierat.Domain;
using Microsoft.Extensions.Configuration;

namespace EvidenciaDomacichZvierat.Data
{
    public class MajitelRepository : IMajitelRepository
    {
        private string _connectionString;

        public MajitelRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default") ?? throw new NullReferenceException("Default connection string not configured.");
        }

        public async Task<IEnumerable<Majitel>> GetAll()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT Id,Meno,Priezvisko,DatumNarodenia FROM dbo.Majitel", connection);
            var reader = await command.ExecuteReaderAsync();

            var result = new List<Majitel>();

            while (await reader.ReadAsync())
                result.Add(reader.ParseMajitel());

            return result;
        }

        public async Task<double> GetPriemernyVekZvieratNaMajitela(params int[] majitelIds)
        {
            var sqlTemplate = @"SELECT AVG(Cast(Datediff(""yyyy"", z.DatumNarodenia, getdate()) as Float)) FROM Zviera z 
                                JOIN MajitelZviera mz ON mz.ZvieraId = z.Id
                                WHERE mz.MajitelId IN ({0})";

            return await SelectFirstValueWhereIn<double>(sqlTemplate, majitelIds);
        }

        public async Task<double> GetPriemernyPocetZvieratNaMajitela(params int[] majitelIds)
        {
            var sqlTemplate = @"SELECT AVG(Cast(pocet as Float)) FROM (
	                                SELECT COUNT(*) as pocet FROM Zviera z
	                                JOIN MajitelZviera mz ON mz.ZvieraId=z.Id
	                                GROUP BY mz.MajitelId
	                                HAVING mz.MajitelId IN ({0})
	                            ) as pocty";

            return await SelectFirstValueWhereIn<double>(sqlTemplate, majitelIds);
        }

        public async Task<double> GetPocetZvieratOdMajitelov(params int[] majitelIds)
        {
            var sqlTemplate = @"SELECT COUNT(DISTINCT(z.Id)) as PocetZvierat FROM Zviera z
                                JOIN MajitelZviera mz ON mz.ZvieraId=z.Id
                                WHERE mz.MajitelId IN ({0})";

            return await SelectFirstValueWhereIn<int>(sqlTemplate, majitelIds);
        }

        public async Task<Majitel> GetById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = "SELECT Id,Meno,Priezvisko,DatumNarodenia from dbo.Majitel WHERE Id = @Id";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
                return null;

            await reader.ReadAsync();
            var majitel = reader.ParseMajitel();

            await FillZvierata(majitel, connection);
            return majitel;
        }

        public async Task Add(Majitel majitel)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var insertSql = "INSERT INTO dbo.Majitel(Meno,Priezvisko,DatumNarodenia) VALUES (@Meno, @Priezvisko, @DatumNarodenia); SELECT SCOPE_IDENTITY();";

            var command = new SqlCommand(insertSql, connection);
            var transaction = connection.BeginTransaction("AddMajitel");

            command.Transaction = transaction;
            command.Parameters.AddWithValue("@Meno", majitel.Meno);
            command.Parameters.AddWithValue("@Priezvisko", majitel.Priezvisko);
            command.Parameters.AddWithValue("@DatumNarodenia", majitel.DatumNarodenia);

            var majitelId = Convert.ToInt32(await command.ExecuteScalarAsync());

            foreach (var zviera in majitel.Zvierata.Where(x => x.Id != default))
            {
                await CreateMajitelZvieraRelation(command, majitelId, zviera.Id);
            }

            await transaction.CommitAsync();
        }

        private async Task<T> SelectFirstValueWhereIn<T>(string sqlTemplate, int[] ids)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = SqlWhereInParamBuilder.BuildWhereInClause(sqlTemplate, "Param", ids);
            var command = new SqlCommand(sql, connection);
            command.AddParamsToCommand("Param", ids);

            using var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();
            return (T)reader.GetValue(0);
        }

        private async Task CreateMajitelZvieraRelation(SqlCommand command, int majitelId, int zvieraId)
        {
            command.CommandText = "INSERT INTO dbo.MajitelZviera(MajitelId, ZvieraId) VALUES(@MajitelId, @ZvieraId)";

            command.Parameters.Clear();
            command.Parameters.AddWithValue("@MajitelId", majitelId);
            command.Parameters.AddWithValue("@ZvieraId", zvieraId);

            await command.ExecuteNonQueryAsync();
        }

        private async Task FillZvierata(Majitel majitel, SqlConnection connection)
        {
            var sql = @"SELECT Discriminator,z.Id,z.Meno,PocetKrmeni,z.DatumNarodenia,UrovenVycviku,PredpokladanyVzrast,ChytaMysi
                        FROM dbo.Zviera z JOIN dbo.MajitelZviera mz ON z.Id=mz.ZvieraId JOIN dbo.Majitel m ON m.Id=mz.MajitelId 
                        WHERE m.Id=@MajitelId
                        ORDER BY z.Meno";

            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MajitelId", majitel.Id);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                majitel.Zvierata.Add(reader.ParseZviera());
        }
    }
}
