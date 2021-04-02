using System.Data.SqlClient;
using EvidenciaDomacichZvierat.Domain;

namespace EvidenciaDomacichZvierat.Data.Helpers
{
    public static class MajitelParser
    {
        public static Majitel ParseMajitel(this SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var meno = reader.GetString(reader.GetOrdinal("Meno"));
            var priezvisko = reader.GetString(reader.GetOrdinal("Priezvisko"));
            var datum = reader.GetDateTime(reader.GetOrdinal("DatumNarodenia"));

            return new Majitel(meno, priezvisko, datum) { Id = id };
        }
    }
}
