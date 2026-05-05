using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services
{
    public class MovimentacaoEstoqueService:IMovimentacaoEstoqueService
    {
        private readonly IMovimentacaoEstoqueRepository _movimentacaoRepository;
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        public MovimentacaoEstoqueService(IMovimentacaoEstoqueRepository movimentacaoRepository,IEstoqueRepository estoqueRepository,
            IProdutoRepository produtoRepository,IMapper mapper)
        {
            _movimentacaoRepository = movimentacaoRepository;
            _estoqueRepository = estoqueRepository;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        public void Registrar(MovimentacaoEstoqueDto dto)
        {
            if (dto.Quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero.");

            if (dto.Tipo != "ENTRADA" && dto.Tipo != "SAIDA")
                throw new ArgumentException("Tipo de movimentação inválido.");

            var produto = _produtoRepository.BuscarPorId(dto.ProdutoId);

            if (produto == null)
                throw new ArgumentException("Produto não encontrado.");

            var estoque = _estoqueRepository.ObterPorProdutoId(dto.ProdutoId);

            if (dto.Tipo == "SAIDA")
            {
                if (estoque == null || estoque.Quantidade < dto.Quantidade)
                    throw new InvalidOperationException("Estoque insuficiente.");
            }

            var movimentacao = _mapper.Map<MovimentacaoEstoque>(dto);

            _movimentacaoRepository.Inserir(movimentacao);

            if (estoque == null)
            {
                estoque = new Estoque
                {
                    ProdutoId = dto.ProdutoId,
                    Quantidade = dto.Tipo == "ENTRADA" ? dto.Quantidade : 0
                };

                _estoqueRepository.Inserir(estoque);
                return;
            }

            if (dto.Tipo == "ENTRADA")
                estoque.Quantidade += dto.Quantidade;

            if (dto.Tipo == "SAIDA")
                estoque.Quantidade -= dto.Quantidade;

            _estoqueRepository.Atualizar(estoque);
        }

        public MovimentacaoEstoqueResponseDto BuscarPorId(int id)
        {
            var movimentacao = _movimentacaoRepository.BuscarPorId(id);

            if (movimentacao == null)
                throw new ArgumentException("Movimentação não encontrada.");

            return _mapper.Map<MovimentacaoEstoqueResponseDto>(movimentacao);
        }

        public IEnumerable<MovimentacaoEstoqueResponseDto> ListarPorProduto(int produtoId)
        {
            var movimentacoes = _movimentacaoRepository.ListarPorProduto(produtoId);

            return _mapper.Map<IEnumerable<MovimentacaoEstoqueResponseDto>>(movimentacoes);
        }

        public IEnumerable<MovimentacaoEstoqueResponseDto> ListarTodos()
        {
            var movimentacoes = _movimentacaoRepository.ListarTodos();

            return _mapper.Map<IEnumerable<MovimentacaoEstoqueResponseDto>>(movimentacoes);
        }
    }
}
