using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EvidenciaDomacichZvierat.Data;
using EvidenciaDomacichZvierat.Exceptions;
using MediatR;

namespace EvidenciaDomacichZvierat.Features.Majitel
{
    public class GetMajitelDetail
    {
        public class Query : IRequest<MajitelDetailDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, MajitelDetailDto>
        {
            private IMajitelRepository _majitelRepository;

            public Handler(IMajitelRepository majitelRepository)
            {
                _majitelRepository = majitelRepository;
            }

            public async Task<MajitelDetailDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var majitel = await _majitelRepository.GetById(request.Id);

                if (majitel is null)
                    throw new NotFoundException($"Majitel with Id of {{{request.Id}}} was not found.");

                var dto = new MajitelDetailDto
                {
                    Id = majitel.Id,
                    Meno = majitel.Meno,
                    Priezvisko = majitel.Priezvisko,
                    Vek = DateTime.Now.Subtract(majitel.DatumNarodenia).Days / 365,
                    PriemernyVekZvierat = await _majitelRepository.GetPriemernyVekZvierata(majitel.Id)
                };

                majitel.Psy.ForEach(x => dto.Zvierata.Add(new MajitelDetailDto.ZvieraDto
                {
                    Id = x.Id,
                    DatumNarodenia = x.DatumNarodenia,
                    Meno = x.Meno,
                    PocetKrmeni = x.PocetKrmeni,
                    UrovenVycviku = x.UrovenVycviku,
                    PredpokladanyVzrastCm = x.PredpokladanyVzrastCm,
                    Type = MajitelDetailDto.ZvieraEnum.Pes
                }));

                majitel.Macky.ForEach(x => dto.Zvierata.Add(new MajitelDetailDto.ZvieraDto
                {
                    Id = x.Id,
                    DatumNarodenia = x.DatumNarodenia,
                    Meno = x.Meno,
                    PocetKrmeni = x.PocetKrmeni,
                    ChytaMysi = x.ChytaMysi,
                    Type = MajitelDetailDto.ZvieraEnum.Macka
                }));

                dto.Zvierata.OrderBy(x => x.Meno);
                return dto;
            }
        }

        public class MajitelDetailDto
        {
            public int Id { get; set; }

            public string Meno { get; set; }

            public string Priezvisko { get; set; }

            public double PriemernyVekZvierat { get; set; }

            public int Vek { get; set; }

            public List<ZvieraDto> Zvierata { get; set; } = new();

            public class ZvieraDto
            {
                public int Id { get; set; }

                public string Meno { get; set; }

                public int PocetKrmeni { get; set; }

                public DateTime DatumNarodenia { get; set; }

                public int? UrovenVycviku { get; set; }

                public int? PredpokladanyVzrastCm { get; set; }

                public bool? ChytaMysi { get; set; }

                public ZvieraEnum Type { get; set; }
            }

            public enum ZvieraEnum
            {
                Pes,
                Macka
            }
        }
    }
}
