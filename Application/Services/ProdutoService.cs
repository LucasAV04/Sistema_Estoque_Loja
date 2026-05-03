using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Infrastructure.Repositories.Interfaces;
namespace Application.Services
{
    public class ProdutoService:IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
        public ProdutoService(IProdutoRepository produtoRepository, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        public void CriarProduto(ProdutoCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Ref))
                throw new ArgumentException("Nome e a Ref são Obrigatórios");

            if (dto.Valor_Venda <= 0)
                throw new Exception("Valor de venda inválido");

            var produtoExistente = _produtoRepository.BuscarPorRef(dto.Ref);
            if (produtoExistente != null)
                throw new InvalidOperationException("Já existe um produto cadastrado com essa Ref");

            var produto = _mapper.Map<Produto>(dto);
            _produtoRepository.InserirProduto(produto);
        }

        public ProdutoResponseDto BuscarPorId(int id)
        {
            var produto = _produtoRepository.BuscarPorId(id);
            if (produto == null)
                throw new ArgumentException("Produto não Encontrado");

            return _mapper.Map<ProdutoResponseDto>(produto);
        }

        public ProdutoResponseDto BuscarPorRef(string Ref)
        {
            var produto = _produtoRepository.BuscarPorRef(Ref);
            if (produto == null)
                throw new ArgumentException("Produto não Encontrado");
            return _mapper.Map<ProdutoResponseDto>(produto);
        }

        public IEnumerable<ProdutoResponseDto> Buscar(string? nome,string? Ref)
        {
            var produtos = _produtoRepository.Buscar(nome, Ref);
            return _mapper.Map<IEnumerable<ProdutoResponseDto>>(produtos);
        }
        
        public List<ProdutoResponseDto> ListarTodos()
        {
            var produtos = _produtoRepository.ListarProdutos();
            return _mapper.Map<List<ProdutoResponseDto>>(produtos);
        }

        public void AtualizarProduto(int id,ProdutoCreateDto dto)
        {
            var produto = _produtoRepository.BuscarPorId(id);
            if (produto == null)
                throw new ArgumentException("Produto não encontrado");
            _mapper.Map(dto, produto);
            _produtoRepository.AtualizarProduto(produto);
        }

        public void DeletarProduto(int id)
        {
            var produto = _produtoRepository.BuscarPorId(id);
            if (produto == null)
                throw new ArgumentException("Produto não encontrado");

            _produtoRepository.DeletarProduto(produto);
        }
    }
}
