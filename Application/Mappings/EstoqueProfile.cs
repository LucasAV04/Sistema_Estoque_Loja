using Application.DTOs;
using AutoMapper;
using Domain;

namespace Application.Mappings
{
    public class EstoqueProfile:Profile
    {
        public EstoqueProfile()
        {
            CreateMap<EstoqueDetalhado, EstoqueDetalhadoResponseDto>();
        }
    }
}
