using Application.DTOs;
using Application.Interfaces;
using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services
{
    public class VendaService:IVendaService
    {
        private readonly IVendaRepository _vendaRepository;
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IProdutoRepository _produtoRepository;

        public VendaService(
            IVendaRepository vendaRepository,
            IEstoqueRepository estoqueRepository,
            IProdutoRepository produtoRepository)
        {
            _vendaRepository = vendaRepository;
            _estoqueRepository = estoqueRepository;
            _produtoRepository = produtoRepository;
        }

        public VendaResponseDto FinalizarVenda(FinalizarVendaDto dto)
        {
            if (dto.Itens == null || !dto.Itens.Any())
                throw new ArgumentException("A venda deve conter ao menos um item.");

           
            foreach (var item in dto.Itens)
            {
                var produto = _produtoRepository.BuscarPorId(item.ProdutoId);
                if (produto == null)
                    throw new ArgumentException($"Produto {item.ProdutoId} não encontrado.");

                var estoque = _estoqueRepository.ObterPorProdutoId(item.ProdutoId);
                if (estoque == null || estoque.Quantidade < item.Quantidade)
                    throw new InvalidOperationException(
                        $"Estoque insuficiente para o produto '{item.NomeProduto}'.");
            }

            var venda = new Venda
            {
                Data = DateTime.Now,
                ValorTotal = dto.Itens.Sum(i => i.Quantidade * i.ValorUnitario)
            };

            var vendaId = _vendaRepository.Inserir(venda);
            venda.Id = vendaId;

            foreach (var itemDto in dto.Itens)
            {
                var vendaItem = new VendaItem
                {
                    VendaId = vendaId,
                    ProdutoId = itemDto.ProdutoId,
                    NomeProduto = itemDto.NomeProduto,
                    RefProduto = itemDto.RefProduto,
                    Quantidade = itemDto.Quantidade,
                    ValorUnitario = itemDto.ValorUnitario
                };

                _vendaRepository.InserirItem(vendaItem);

               
                var estoque = _estoqueRepository.ObterPorProdutoId(itemDto.ProdutoId);
                estoque!.Quantidade -= itemDto.Quantidade;
                _estoqueRepository.Atualizar(estoque);
            }

            return MapearParaDto(venda, dto.Itens);
        }

        public IEnumerable<VendaResponseDto> ListarTodos()
        {
            var vendas = _vendaRepository.ListarTodos();
            return vendas.Select(v => new VendaResponseDto
            {
                Id = v.Id,
                Data = v.Data,
                ValorTotal = v.ValorTotal,
                Itens = v.Itens.Select(i => new VendaItemResponseDto
                {
                    ProdutoId = i.ProdutoId,
                    NomeProduto = i.NomeProduto,
                    RefProduto = i.RefProduto,
                    Quantidade = i.Quantidade,
                    ValorUnitario = i.ValorUnitario,
                    ValorTotal = i.ValorTotal
                }).ToList()
            });
        }

        public VendaResponseDto BuscarPorId(int id)
        {
            var venda = _vendaRepository.BuscarPorId(id);
            if (venda == null)
                throw new ArgumentException("Venda não encontrada.");

            return new VendaResponseDto
            {
                Id = venda.Id,
                Data = venda.Data,
                ValorTotal = venda.ValorTotal,
                Itens = venda.Itens.Select(i => new VendaItemResponseDto
                {
                    ProdutoId = i.ProdutoId,
                    NomeProduto = i.NomeProduto,
                    RefProduto = i.RefProduto,
                    Quantidade = i.Quantidade,
                    ValorUnitario = i.ValorUnitario,
                    ValorTotal = i.ValorTotal
                }).ToList()
            };
        }

        private VendaResponseDto MapearParaDto(Venda venda, List<VendaItemDto> itens)
        {
            return new VendaResponseDto
            {
                Id = venda.Id,
                Data = venda.Data,
                ValorTotal = venda.ValorTotal,
                Itens = itens.Select(i => new VendaItemResponseDto
                {
                    ProdutoId = i.ProdutoId,
                    NomeProduto = i.NomeProduto,
                    RefProduto = i.RefProduto,
                    Quantidade = i.Quantidade,
                    ValorUnitario = i.ValorUnitario,
                    ValorTotal = i.Quantidade * i.ValorUnitario
                }).ToList()
            };
        }

        public void Deletar(int id)
        {
            var venda = _vendaRepository.BuscarPorId(id);
            if (venda == null)
                throw new ArgumentException("Venda não encontrada.");

            _vendaRepository.Deletar(id);
        }
    }
}
