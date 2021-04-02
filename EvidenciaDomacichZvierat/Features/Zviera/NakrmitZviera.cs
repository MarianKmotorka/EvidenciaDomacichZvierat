using System.Threading;
using System.Threading.Tasks;
using EvidenciaDomacichZvierat.Data;
using MediatR;

namespace EvidenciaDomacichZvierat.Features.Zviera
{
    public class NakrmitZviera
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private IZvieraRepository _zvieraRepository;

            public Handler(IZvieraRepository zvieraRepository)
            {
                _zvieraRepository = zvieraRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await _zvieraRepository.Nakrmit(request.Id);
                return Unit.Value;
            }
        }
    }
}
