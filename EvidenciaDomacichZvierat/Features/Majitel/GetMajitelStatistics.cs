using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EvidenciaDomacichZvierat.Data;
using MediatR;

namespace EvidenciaDomacichZvierat.Features.Majitel
{
    public class GetMajitelStatistics
    {
        public class Query : IRequest<Response>
        {
            public int[] Ids { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private IMajitelRepository _majitelRepository;

            public Handler(IMajitelRepository majitelRepository)
            {
                _majitelRepository = majitelRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request.Ids is null || !request.Ids.Any())
                    return new();

                return new()
                {
                    PriemernyPocetZvieratNaMajitela = await _majitelRepository.GetPriemernyPocetZvieratNaMajitela(request.Ids),
                    PriemernyVekZvieratNaMajitela = await _majitelRepository.GetPriemernyVekZvieratNaMajitela(request.Ids),
                    PocetZvieratOdMajitelov = await _majitelRepository.GetPocetZvieratOdMajitelov(request.Ids)
                };
            }
        }

        public class Response
        {
            public double PriemernyPocetZvieratNaMajitela { get; set; }

            public double PriemernyVekZvieratNaMajitela { get; set; }

            public double PocetZvieratOdMajitelov { get; set; }
        }
    }
}
