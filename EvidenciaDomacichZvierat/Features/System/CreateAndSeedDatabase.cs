using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EvidenciaDomacichZvierat.Data;
using EvidenciaDomacichZvierat.Domain;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace EvidenciaDomacichZvierat.Features.System
{
    public class CreateAndSeedDatabase
    {
        public class Command : IRequest
        {
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IMajitelRepository _majitelRepository;
            private readonly IZvieraRepository _zvieraRepository;
            private readonly IWebHostEnvironment _environment;
            private readonly string _connectionString;

            public Handler(IMajitelRepository majitelRepository, IZvieraRepository zvieraRepository, IWebHostEnvironment environment,
                IConfiguration configuration)
            {
                _environment = environment;
                _zvieraRepository = zvieraRepository;
                _majitelRepository = majitelRepository;
                _connectionString = configuration.GetConnectionString("Default") ?? throw new NullReferenceException("Default connection string not configured.");
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!_environment.IsDevelopment() || await DatabaseExists())
                    return Unit.Value;

                await CreateDatabase();
                await CreateSchema();

                var zvierata = new List<Zviera>()
                {
                     new Macka("Miki", new DateTime(2000,1,1)),
                     new Macka("niki", new DateTime(2000,1,1),true),
                     new Macka("muro", new DateTime(2010,2,4),true),
                     new Macka("Bla", new DateTime(2000,6,6)),
                     new Macka("Blahbla", new DateTime(1999,4,4)),
                     new Macka("Macka1", new DateTime(2004,5,5),true),
                     new Macka("Macka2", new DateTime(2009,8,11)),
                     new Macka("Macka3", new DateTime(2020,5,7)),
                     new Macka("Macka4", new DateTime(2020,5,7),true),
                     new Macka("Macka5", new DateTime(2020,5,7)),
                     new Macka("Macka6", new DateTime(2020,5,7),true),

                     new Pes("Boby",new DateTime(2020,5,7),100,4),
                     new Pes("Bruno",new DateTime(2020,5,7),100,1),
                     new Pes("Dalman",new DateTime(2020,5,7),100,6),
                     new Pes("Denis",new DateTime(2020,5,7),100,9),
                     new Pes("Bodro",new DateTime(2020,5,7),100,10),
                     new Pes("Rocky",new DateTime(2020,5,7),100,1),
                     new Pes("Aldo",new DateTime(2020,5,7),100,1),
                     new Pes("PEs1",new DateTime(2020,5,7),100,3),
                     new Pes("PEs2",new DateTime(2020,5,7),100,3),
                     new Pes("Pes3",new DateTime(2020,5,7),100,3),
                };

                foreach (var zviera in zvierata)
                    await _zvieraRepository.Add(zviera);

                var zvierataDb = await _zvieraRepository.GetAll();

                var boris = new Domain.Majitel("Boris", "Dlhy", new DateTime(1980, 4, 4));
                var aneta = new Domain.Majitel("Aneta", "Uhlova", new DateTime(1980, 4, 4));
                var katarina = new Domain.Majitel("Katarina", "Drobna", new DateTime(1980, 4, 4));
                var stefan = new Domain.Majitel("Stefan", "Zierny", new DateTime(1980, 4, 4));
                var tomas = new Domain.Majitel("Tomas", "Bledy", new DateTime(1980, 4, 4));

                boris.Zvierata.AddRange(new[] { zvierataDb[0], zvierataDb[1], zvierataDb[2], zvierataDb[3] });
                aneta.Zvierata.AddRange(new[] { zvierataDb[2], zvierataDb[4], zvierataDb[6], zvierataDb[7], zvierataDb[10] });
                katarina.Zvierata.AddRange(new[] { zvierataDb[5], zvierataDb[7], zvierataDb[8], zvierataDb[9], zvierataDb[1] });
                stefan.Zvierata.AddRange(new[] { zvierataDb[10], zvierataDb[11], zvierataDb[12], zvierataDb[13] });
                tomas.Zvierata.AddRange(new[] { zvierataDb[14], zvierataDb[15], zvierataDb[16], zvierataDb[17], zvierataDb[18], zvierataDb[19] });

                await _majitelRepository.Add(boris);
                await _majitelRepository.Add(aneta);
                await _majitelRepository.Add(katarina);
                await _majitelRepository.Add(stefan);
                await _majitelRepository.Add(tomas);
                return Unit.Value;
            }

            private async Task CreateDatabase()
            {
                using var connection = new SqlConnection(GetServerConnectionString(_connectionString));
                using var command = new SqlCommand($"CREATE DATABASE {GetDbName()}", connection);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            private async Task CreateSchema()
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(File.ReadAllText(".\\Features\\System\\CreateDbScript.sql"), connection);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            private string GetServerConnectionString(string cnnString)
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(cnnString);
                connectionStringBuilder.Remove("Initial Catalog");
                return connectionStringBuilder.ConnectionString;
            }

            private string GetDbName()
                => new SqlConnectionStringBuilder(_connectionString).InitialCatalog;

            private async Task<bool> DatabaseExists()
            {
                var serverConnectionString = GetServerConnectionString(_connectionString);
                using var connection = new SqlConnection(serverConnectionString);
                using var command = new SqlCommand("SELECT db_id(@databaseName)", connection);

                await connection.OpenAsync();

                command.Parameters.AddWithValue("@databaseName", GetDbName());
                return await command.ExecuteScalarAsync() != DBNull.Value;
            }
        }
    }
}
