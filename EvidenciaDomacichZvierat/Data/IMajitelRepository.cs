using System.Collections.Generic;
using System.Threading.Tasks;
using EvidenciaDomacichZvierat.Domain;

namespace EvidenciaDomacichZvierat.Data
{
    public interface IMajitelRepository
    {
        Task<IEnumerable<Majitel>> GetAll();
    }
}
