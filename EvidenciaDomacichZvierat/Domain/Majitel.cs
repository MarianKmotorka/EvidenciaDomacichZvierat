using System;
using System.Collections.Generic;
using System.Linq;

namespace EvidenciaDomacichZvierat.Domain
{
    public class Majitel
    {
        public int Id { get; set; }

        public string Meno { get; private set; }

        public string Priezvisko { get; private set; }

        public DateTime DatumNarodenia { get; private set; }

        public List<Zviera> Zvierata { get; private set; }

        public List<Pes> Psy => Zvierata.OfType<Pes>().ToList();

        public List<Macka> Macky => Zvierata.OfType<Macka>().ToList();

        public Majitel(string meno, string priezvisko, DateTime datumNarodenia)
        {
            Meno = meno;
            Priezvisko = priezvisko;
            Zvierata = new();
            DatumNarodenia = datumNarodenia;
        }
    }
}
