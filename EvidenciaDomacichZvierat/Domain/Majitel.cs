namespace EvidenciaDomacichZvierat.Domain
{
    public class Majitel
    {
        public int Id { get; set; }

        public string Meno { get; private set; }

        public string Priezvisko { get; private set; }

        public Majitel(string meno, string priezvisko)
        {
            Meno = meno;
            Priezvisko = priezvisko;
        }
    }
}
