using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EvidenciaDomacichZvierat.Data;
using EvidenciaDomacichZvierat.Domain;
using EvidenciaDomacichZvierat.Exceptions;
using MediatR;
using static EvidenciaDomacichZvierat.Features.Majitel.GetMajitelDetail.MajitelDetailDto;

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
                    PriemernyVekZvierat = await _majitelRepository.GetPriemernyVekZvieratOdMajitelov(majitel.Id)
                };

                foreach (var zviera in majitel.Zvierata)
                {
                    var type = (zviera as Pes) is null ? ZvieraEnum.Macka : ZvieraEnum.Pes;

                    if (type == ZvieraEnum.Pes)
                        dto.Zvierata.Add(new ZvieraDto
                        {
                            Id = zviera.Id,
                            DatumNarodenia = zviera.DatumNarodenia,
                            Meno = zviera.Meno,
                            PocetKrmeni = zviera.PocetKrmeni,
                            UrovenVycviku = ((Pes)zviera).UrovenVycviku,
                            PredpokladanyVzrastCm = ((Pes)zviera).PredpokladanyVzrastCm,
                            Type = type
                        });
                    else
                        dto.Zvierata.Add(new ZvieraDto
                        {
                            Id = zviera.Id,
                            DatumNarodenia = zviera.DatumNarodenia,
                            Meno = zviera.Meno,
                            PocetKrmeni = zviera.PocetKrmeni,
                            ChytaMysi = ((Macka)zviera).ChytaMysi,
                            Type = type
                        });
                }

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
