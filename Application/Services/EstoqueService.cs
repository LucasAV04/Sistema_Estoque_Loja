using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services
{
    public class EstoqueService : IEstoqueService
    {
        private readonly IEstoqueRepository _repo;
        private readonly IMapper _mapper;
        public EstoqueService(IEstoqueRepository repo,IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public void Entrada(EntradaEstoqueDto dto)
        {
            if (dto.Quantidade <= 0)
                throw new ArgumentOutOfRangeException(nameof(dto.Quantidade), "Quantidade deve ser maior que zero.");

            var estoque = _repo.ObterPorProdutoId(dto.ProdutoId);
            if (estoque == null)
            {
                estoque = new Estoque
                {
                    ProdutoId = dto.ProdutoId,
                    Quantidade = dto.Quantidade,
                };
                _repo.Inserir(estoque);
            }
            else
            {
                estoque.Quantidade += dto.Quantidade;
                _repo.Atualizar(estoque);
            }
        }

        public void Saida(SaidaEstoqueDto dto)
        {
            if (dto.Quantidade <= 0)
                throw new ArgumentOutOfRangeException(nameof(dto.Quantidade), "Quantidade deve ser maior que zero.");

            var estoque = _repo.ObterPorProdutoId(dto.ProdutoId);
            if (estoque == null || estoque.Quantidade < dto.Quantidade)
                throw new InvalidOperationException("Estoque insuficiente.");

            estoque.Quantidade -= dto.Quantidade;
            _repo.Atualizar(estoque);
        }

        public EstoqueResponseDto ObterPorProduto(int produtoId)
        {
            var estoque = _repo.ObterPorProdutoId(produtoId);
            if (estoque == null)
            {
                return new EstoqueResponseDto
                {
                    ProdutoId = produtoId,
                    Quantidade = 0
                };
            }

            return new EstoqueResponseDto
            {
                ProdutoId = estoque.ProdutoId,
                Quantidade = estoque.Quantidade
            };
        }
        public IEnumerable<EstoqueDetalhadoResponseDto> ListarDetalhado()
        {
            var estoque = _repo.ListarEstoqueDetalhado();

            return _mapper.Map<IEnumerable<EstoqueDetalhadoResponseDto>>(estoque);
        }
    }
}   