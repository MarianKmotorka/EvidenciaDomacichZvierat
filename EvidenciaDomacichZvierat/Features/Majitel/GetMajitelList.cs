using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EvidenciaDomacichZvierat.Data;
using MediatR;

namespace EvidenciaDomacichZvierat.Features.Majitel
{
    public class GetMajitelList
    {
        public class Query : IRequest<List<MajitelDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<MajitelDto>>
        {
            private IMajitelRepository _majitelRepository;

            public Handler(IMajitelRepository majitelRepository)
            {
                _majitelRepository = majitelRepository;
            }

            public async Task<List<MajitelDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var majitelia = await _majitelRepository.GetAll();

                return majitelia.Select(x => new MajitelDto
                {
                    Id = x.Id,
                    Meno = x.Meno,
                    Priezvisko = x.Priezvisko
                }).ToList();
            }
        }

        public class MajitelDto
        {
            public int Id { get; set; }

            public string Meno { get; set; }

            public string Priezvisko { get; set; }
        }
    }
}
