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

                var zvierata = new List<Domain.Zviera>()
                {
                     new Macka("Miki", new DateTime(2003,4,1)),
                     new Macka("Niki", new DateTime(2002,1,1),true),
                     new Macka("Muro", new DateTime(2010,2,4),true),
                     new Macka("Bella", new DateTime(2010,6,6)),
                     new Macka("Kitty", new DateTime(1999,4,4)),
                     new Macka("Lilly", new DateTime(2004,5,5),true),
                     new Macka("Charlie", new DateTime(2009,8,11)),
                     new Macka("Milo", new DateTime(2020,5,7)),
                     new Macka("Leo", new DateTime(2020,5,7),true),
                     new Macka("Chuck", new DateTime(2016,5,7)),
                     new Macka("Perry", new DateTime(2011,5,7),true),

                     new Pes("Cooper",new DateTime(2010,5,7),100,4),
                     new Pes("Daisy",new DateTime(2011,4,7),50,1),
                     new Pes("Dalman",new DateTime(2019,5,10),20,6),
                     new Pes("Denis",new DateTime(2018,10,7),90,9),
                     new Pes("Bodro",new DateTime(2000,5,17),100,10),
                     new Pes("Rocky",new DateTime(2011,5,7),44,1),
                     new Pes("Aldo",new DateTime(1999,5,13),90,1),
                     new Pes("Zuck",new DateTime(1998,5,5),100,3),
                     new Pes("Yup",new DateTime(2014,5,7),100,3),
                     new Pes("Dino",new DateTime(2012,8,7),100,3),
                };

                foreach (var zviera in zvierata)
                    await _zvieraRepository.Add(zviera);

                var zvierataDb = await _zvieraRepository.GetAll();

                var boris = new Domain.Majitel("Boris", "Dlhy", new DateTime(1980, 4, 4));
                var aneta = new Domain.Majitel("Aneta", "Uhlova", new DateTime(1990, 11, 11));
                var katarina = new Domain.Majitel("Katarina", "Drobna", new DateTime(1970, 4, 4));
                var stefan = new Domain.Majitel("Stefan", "Zierny", new DateTime(1953, 5, 4));
                var tomas = new Domain.Majitel("Tomas", "Bledy", new DateTime(2000, 4, 10));

                boris.Zvierata.AddRange(new[] { zvierataDb[0], zvierataDb[10], zvierataDb[19], zvierataDb[3], zvierataDb[11], zvierataDb[14] });
                aneta.Zvierata.AddRange(new[] { zvierataDb[2], zvierataDb[4], zvierataDb[6], zvierataDb[18] });
                katarina.Zvierata.AddRange(new[] { zvierataDb[5], zvierataDb[7], zvierataDb[8], zvierataDb[9], zvierataDb[1], zvierataDb[15] });
                stefan.Zvierata.AddRange(new[] { zvierataDb[1], zvierataDb[2], zvierataDb[12], zvierataDb[13] });
                tomas.Zvierata.AddRange(new[] { zvierataDb[4], zvierataDb[3], zvierataDb[16], zvierataDb[17], zvierataDb[0], zvierataDb[19] });

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
