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

        //public async Task<Majitel> GetById(int id)
        //{
        //    using var connection = new SqlConnection(_connectionString);
        //    await connection.OpenAsync();

        //    var command = new SqlCommand("SELECT Id,Meno,Priezvisko from dbo.Majitel", connection);
        //    var reader = await command.ExecuteReaderAsync();

        //    var result = new List<Majitel>();

        //    while (await reader.ReadAsync())
        //    {
        //        result.Add(new Majitel(reader.GetString(1), reader.GetString(2))
        //        {
        //            Id = reader.GetInt32(0)
        //        });
        //    }

        //    return result;
        //}
    }
}
