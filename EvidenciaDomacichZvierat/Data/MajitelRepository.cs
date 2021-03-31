using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

            var command = new SqlCommand("SELECT Id,Meno,Priezvisko from dbo.Majitel", connection);
            var reader = await command.ExecuteReaderAsync();

            var result = new List<Majitel>();

            while (await reader.ReadAsync())
            {
                result.Add(new Majitel(reader.GetString(1), reader.GetString(2))
                {
                    Id = reader.GetInt32(0)
                });
            }

            return result;
        }

        public async Task<Majitel> GetById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = "SELECT Id,Meno,Priezvisko from dbo.Majitel WHERE Id = @Id";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            var reader = await command.ExecuteReaderAsync();


            if (!reader.HasRows)
                return null;

            await reader.ReadAsync();

            var majitel = new Majitel(reader.GetString(1), reader.GetString(2)) { Id = reader.GetInt32(0) };
            await FillZvierata(majitel, connection);
            return majitel;
        }

        private async Task FillZvierata(Majitel majitel, SqlConnection connection)
        {
            var sql = @"SELECT Discriminator,z.Id,z.Meno,PocetKrmeni,DatumNarodenia,UrovenVycviku,PredpokladanyVzrast,ChytaMysi
                        FROM dbo.Zviera z JOIN dbo.MajitelZviera mz ON z.Id=mz.ZvieraId JOIN dbo.Majitel m ON m.Id=mz.MajitelId 
                        WHERE m.Id=@MajitelId";

            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MajitelId", majitel.Id);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var discriminator = reader.GetString(0);

                switch (discriminator)
                {
                    case nameof(Pes):
                        majitel.Zvierata.Add(new Pes(reader.GetString(2), reader.GetDateTime(4), reader.GetInt32(6), reader.GetInt32(5))
                        {
                            Id = reader.GetInt32(1),
                        });
                        break;

                    case nameof(Macka):
                        majitel.Zvierata.Add(new Macka(reader.GetString(2), reader.GetDateTime(4), reader.GetBoolean(7))
                        {
                            Id = reader.GetInt32(1),
                        });
                        break;

                    default:
                        throw new NotSupportedException($"dbo.Zviera.Discriminator s hodnotou ${discriminator} nie je podporovany.");

                }
            }
        }
    }
}
