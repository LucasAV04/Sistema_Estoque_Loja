using AutoMapper;
using Domain;

namespace Application.DTOs
{
    public  class MovimentacaoEstoqueProfile:Profile
    {
        public MovimentacaoEstoqueProfile()
        {
            CreateMap<MovimentacaoEstoqueDto, MovimentacaoEstoque>()
                .ForMember(dest => dest.Created_At, opt => opt.Ignore());

            CreateMap<MovimentacaoEstoque, MovimentacaoEstoqueResponseDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Created_At));
        }
    }
}
