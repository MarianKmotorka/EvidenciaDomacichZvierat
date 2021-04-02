using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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
            {
                result.Add(new Majitel(reader.GetString(1), reader.GetString(2), reader.GetDateTime(3))
                {
                    Id = reader.GetInt32(0)
                });
            }

            return result;
        }

        public async Task<double> GetPriemernyVekZvierata(int majitelId)
        {
            var sql = @"select AVG( Cast( Datediff(""yyyy"", z.DatumNarodenia, getdate()) as Float) ) FROM Zviera z 
                        JOIN MajitelZviera mz ON mz.ZvieraId = z.Id
                        WHERE mz.MajitelId = @MajitelId";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MajitelId", majitelId);

            var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();

            return reader.GetDouble(0);
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

            var majitel = new Majitel(reader.GetString(1), reader.GetString(2), reader.GetDateTime(3)) { Id = reader.GetInt32(0) };
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
            {
                var discriminator = reader.GetString(0);
                Zviera zviera = null;

                zviera = discriminator switch
                {
                    nameof(Pes) => new Pes(reader.GetString(2), reader.GetDateTime(4), reader.GetInt32(6), reader.GetInt32(5)) { Id = reader.GetInt32(1) },
                    nameof(Macka) => new Macka(reader.GetString(2), reader.GetDateTime(4), reader.GetBoolean(7)) { Id = reader.GetInt32(1) },
                    _ => throw new NotSupportedException($"dbo.Zviera.Discriminator s hodnotou ${discriminator} nie je podporovany."),
                };

                zviera.SetPocetKrmeni(reader.GetInt32(3));
                majitel.Zvierata.Add(zviera);
            }
        }
    }
}
