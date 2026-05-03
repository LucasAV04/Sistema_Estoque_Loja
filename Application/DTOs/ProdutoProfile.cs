using AutoMapper;
using Domain;

namespace Application.DTOs
{
    public class ProdutoProfile:Profile
    {
        public ProdutoProfile()
        {
            CreateMap<ProdutoCreateDto,Produto>();
            CreateMap<ProdutoResponseDto, Produto>();
        }
    }
}
