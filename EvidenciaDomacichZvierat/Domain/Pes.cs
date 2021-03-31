using System;

namespace EvidenciaDomacichZvierat.Domain
{
    public class Pes : Zviera
    {
        public Pes(string meno, DateTime datumNarodenia, int predpokladanyVzrast, byte urovenVycviku) : base(meno, datumNarodenia)
        {
            SetUrovenVycviku(urovenVycviku);
            SetPredpokladanyVzrastCm(predpokladanyVzrast);
        }

        public Pes(string meno, DateTime datumNarodenia, int predpokladanyVzrast) : this(meno, datumNarodenia, predpokladanyVzrast, 1)
        {
            UrovenVycviku = 1;
        }

        public Pes(string meno, DateTime datumNarodenia) : this(meno, datumNarodenia, 0)
        {
            UrovenVycviku = 1;
        }

        public byte UrovenVycviku { get; private set; }

        public int PredpokladanyVzrastCm { get; private set; }

        public void SetUrovenVycviku(byte value)
        {
            if (value < 1 || value > 10)
                throw new ArgumentException("UrovenVycviku musi byt od 1 do 10.");

            UrovenVycviku = value;
        }

        public void SetPredpokladanyVzrastCm(int value)
        {
            if (value < 0)
                throw new ArgumentException("PredpokladanyVzrastCm musi byt viac ako 0cm.");

            PredpokladanyVzrastCm = value;
        }
    }
}
