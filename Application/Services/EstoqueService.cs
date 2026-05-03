using Application.DTOs;
using Application.Interfaces;
using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services
{
    public class EstoqueService:IEstoqueService
    {
        private readonly IEstoqueRepository _repo;

        public EstoqueService(IEstoqueRepository repo)
        {
            _repo = repo;
        }

        public void Entrada(EntradaEstoqueDto dto)
        {
           if(dto.Quantidade <= 0)
               throw new ArgumentOutOfRangeException("Quantidade Insuficiente");

           var estoque = _repo.ObterPorProdutoId(dto.ProdutoId);
            if(estoque == null)
            {
                estoque = new Estoque
                {
                    Produto_Id = dto.ProdutoId,
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
                throw new ArgumentOutOfRangeException("Quantidade Insuficiente");

            var estoque = _repo.ObterPorProdutoId(dto.ProdutoId);
            if(estoque == null || estoque.Quantidade < dto.Quantidade)
                throw new Exception("Estoque Insuficiente");

            estoque.Quantidade -= dto.Quantidade;
            _repo.Atualizar(estoque);
        }

        public EstoqueResponseDto ObterPorProduto(int produtoId)
        {
            var estoque = _repo.ObterPorProdutoId(produtoId);
            if(estoque == null)
            {
                return new EstoqueResponseDto
                {
                    ProdutoId = produtoId,
                    Quantidade = 0
                };
            }

            return new EstoqueResponseDto
            {
                ProdutoId = estoque.Produto_Id,
                Quantidade = estoque.Quantidade
            };
        }
    }
}
