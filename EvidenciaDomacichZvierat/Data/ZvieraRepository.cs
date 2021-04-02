using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using EvidenciaDomacichZvierat.Data.Helpers;
using EvidenciaDomacichZvierat.Domain;
using Microsoft.Extensions.Configuration;

namespace EvidenciaDomacichZvierat.Data
{
    public class ZvieraRepository : IZvieraRepository
    {
        private string _connectionString;

        public ZvieraRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default") ?? throw new NullReferenceException("Default connection string not configured.");
        }

        public async Task Add(Zviera zviera)
        {
            var isPes = (zviera as Pes) is not null;
            var isMacka = (zviera as Macka) is not null;

            if (!isMacka && !isPes)
                throw new NotSupportedException($"Zviera typu {zviera.GetType()} nie je podporovane.");

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var insertSql = @"INSERT INTO dbo.Zviera(Meno, PocetKrmeni, DatumNarodenia, UrovenVycviku, PredpokladanyVzrast, ChytaMysi, Discriminator)
                              VALUES (@Meno, @PocetKrmeni, @DatumNarodenia, @UrovenVycviku, @PredpokladanyVzrast, @ChytaMysi, @Discriminator)";

            var command = new SqlCommand(insertSql, connection);
            command.Parameters.AddWithValue("@Meno", zviera.Meno);
            command.Parameters.AddWithValue("@PocetKrmeni", zviera.PocetKrmeni);
            command.Parameters.AddWithValue("@DatumNarodenia", zviera.DatumNarodenia);
            command.Parameters.AddWithValue("@UrovenVycviku", isPes ? (zviera as Pes).UrovenVycviku : DBNull.Value);
            command.Parameters.AddWithValue("@PredpokladanyVzrast", isPes ? (zviera as Pes).PredpokladanyVzrastCm : DBNull.Value);
            command.Parameters.AddWithValue("@ChytaMysi", isMacka ? (zviera as Macka).ChytaMysi : DBNull.Value);
            command.Parameters.AddWithValue("@Discriminator", isPes ? nameof(Pes) : nameof(Macka));
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Zviera>> GetAll()
        {
            var sql = "SELECT Discriminator,Id,Meno,PocetKrmeni,DatumNarodenia,UrovenVycviku,PredpokladanyVzrast,ChytaMysi FROM dbo.Zviera";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(sql, connection);
            var reader = await command.ExecuteReaderAsync();

            var result = new List<Zviera>();

            while (await reader.ReadAsync())
                result.Add(reader.ParseZviera());

            return result;
        }

        public async Task Nakrmit(int id)
        {
            var sql = "UPDATE Zviera SET PocetKrmeni = PocetKrmeni + 1 WHERE Id=@Id";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            await command.ExecuteNonQueryAsync();
        }
    }
}
