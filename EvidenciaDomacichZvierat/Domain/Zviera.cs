using System;

namespace EvidenciaDomacichZvierat.Domain
{
    public abstract class Zviera
    {
        public Zviera(string meno, DateTime datumNarodenia)
        {
            Meno = meno;
            DatumNarodenia = datumNarodenia;
        }

        public int Id { get; set; }

        public string Meno { get; set; }

        public int PocetKrmeni { get; set; }

        public DateTime DatumNarodenia { get; set; }
    }
}
