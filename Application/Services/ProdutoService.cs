using Application.DTOs;
using Application.Interfaces;
using Domain;
using Infrastructure.Repositories.Interfaces;
namespace Application.Services
{
    public class ProdutoService:IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService (IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public void CriarProduto(ProdutoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Ref))
                throw new ArgumentException("Nome e a Ref são Obrigatórios");

            var produtoExistente = _produtoRepository.BuscarPorRef(dto.Ref);
            if (produtoExistente != null)
                throw new InvalidOperationException("Já existe um produto cadastrado com essa Ref");
            if (dto.Valor_Venda <= 0)
                throw new Exception("Valor de venda inválido");
            var produto = new Produto
            {
                Ref = dto.Ref,
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Tipo = dto.Tipo,
                Valor_Compra = dto.Valor_Compra,
                Valor_Venda = dto.Valor_Venda
            };
            _produtoRepository.InserirProduto(produto);
        }
        
    }
}
