using System;

namespace EvidenciaDomacichZvierat.Domain
{
    public class Macka : Zviera
    {
        public Macka(string meno, DateTime datumNarodenia, bool chytaMysi = false) : base(meno, datumNarodenia)
        {
            ChytaMysi = chytaMysi;
        }

        public bool ChytaMysi { get; set; }
    }
}
