using Application.DTOs;
using AutoMapper;
using Domain;

namespace Application.Mappings
{
    public class ProdutoProfile : Profile
    {
        public ProdutoProfile()
        {
            CreateMap<ProdutoCreateDto, Produto>();

           
            CreateMap<Produto, ProdutoResponseDto>();
        }
    }
}