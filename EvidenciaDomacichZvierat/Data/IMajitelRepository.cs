using System.Collections.Generic;
using System.Threading.Tasks;
using EvidenciaDomacichZvierat.Domain;

namespace EvidenciaDomacichZvierat.Data
{
    public interface IMajitelRepository
    {
        Task<IEnumerable<Majitel>> GetAll();

        Task<Majitel> GetById(int id);

        Task Add(Majitel majitel);

        Task<double> GetPriemernyVekZvieratOdMajitelov(params int[] majitelIds);

        Task<double> GetPriemernyPocetZvieratNaMajitela(params int[] majitelIds);

        Task<double> GetPocetZvieratOdMajitelov(params int[] majitelIds);
    }
}
