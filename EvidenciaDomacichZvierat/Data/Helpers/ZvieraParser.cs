using System;
using System.Data.SqlClient;
using EvidenciaDomacichZvierat.Domain;

namespace EvidenciaDomacichZvierat.Data.Helpers
{
    public static class ZvieraParser
    {
        public static Zviera ParseZviera(this SqlDataReader reader)
        {
            var discriminator = reader.GetString(reader.GetOrdinal("Discriminator"));

            var isPes = discriminator == nameof(Pes);
            var isMacka = discriminator == nameof(Macka);

            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var meno = reader.GetString(reader.GetOrdinal("Meno"));
            var pocetKrmeni = reader.GetInt32(reader.GetOrdinal("PocetKrmeni"));
            var datumNarodenia = reader.GetDateTime(reader.GetOrdinal("DatumNarodenia"));
            var urovenVycviku = isPes ? reader.GetInt32(reader.GetOrdinal("UrovenVycviku")) : default;
            var predpokladanyVzrast = isPes ? reader.GetInt32(reader.GetOrdinal("PredpokladanyVzrast")) : default;
            var chytaMysi = isMacka ? reader.GetBoolean(reader.GetOrdinal("ChytaMysi")) : default;

            Zviera zviera = null;

            zviera = discriminator switch
            {
                nameof(Pes) => new Pes(meno, datumNarodenia, predpokladanyVzrast, urovenVycviku) { Id = id },
                nameof(Macka) => new Macka(meno, datumNarodenia, chytaMysi) { Id = id },
                _ => throw new NotSupportedException($"dbo.Zviera.Discriminator s hodnotou ${discriminator} nie je podporovany."),
            };

            zviera.SetPocetKrmeni(pocetKrmeni);
            return zviera;
        }
    }
}
