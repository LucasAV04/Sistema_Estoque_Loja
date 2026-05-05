using AutoMapper;
using Domain;

namespace Application.DTOs
{
    public class MovimentacaoEstoqueProfile : Profile
    {
        public MovimentacaoEstoqueProfile()
        {
            CreateMap<MovimentacaoEstoqueDto, MovimentacaoEstoque>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src =>
                    Enum.Parse<MovimentacaoEstoque.TipoMovimentacao>(src.Tipo.ToUpper())))
                .ForMember(dest => dest.Created_At, opt => opt.Ignore());
                

            CreateMap<MovimentacaoEstoque, MovimentacaoEstoqueResponseDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Created_At))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo.ToString()));
        }
    }
}