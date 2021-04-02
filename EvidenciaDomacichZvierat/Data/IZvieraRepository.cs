using System.Collections.Generic;
using System.Threading.Tasks;
using EvidenciaDomacichZvierat.Domain;

namespace EvidenciaDomacichZvierat.Data
{
    public interface IZvieraRepository
    {
        Task Add(Zviera zviera);

        Task<List<Zviera>> GetAll();

        Task Nakrmit(int id);
    }
}