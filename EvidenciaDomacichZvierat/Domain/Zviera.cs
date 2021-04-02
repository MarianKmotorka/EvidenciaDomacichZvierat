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

        public string Meno { get; private set; }

        public int PocetKrmeni { get; private set; }

        public DateTime DatumNarodenia { get; private set; }

        public void SetPocetKrmeni(int pocet)
        {
            if (pocet < 0)
                throw new InvalidOperationException("Pocet krmeni musi byt vacsi alebo rovny nule.");

            PocetKrmeni = pocet;
        }
    }
}
