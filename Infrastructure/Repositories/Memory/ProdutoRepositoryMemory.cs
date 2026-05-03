using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.Memory
{
    public class ProdutoRepositoryMemory:IProdutoRepository
    {
        private readonly List<Produto> _produtos = new();
        private int proximoId = 1;
        public void InserirProduto(Produto produto)
        {
            _produtos.Add(produto);
            produto.Id = proximoId++;
        }
        public Produto BuscarPorId(int id)
        {
            return _produtos.FirstOrDefault(p => p.Id == id);
        }
        public Produto BuscarPorRef(string Ref)
        {
            return _produtos.FirstOrDefault(p => p.Ref == Ref);
        }
        
        public IEnumerable<Produto> Buscar(string nome, string Ref)
        {
            var query = _produtos.AsEnumerable();

            if(!string.IsNullOrWhiteSpace(nome))
                query = query.Where(p => p.Nome.Contains(nome,StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(Ref))
            {
                query = query.Where(p => p.Ref.Contains(Ref, StringComparison.OrdinalIgnoreCase));
            }
            return query;
        }
        public List<Produto> ListarProdutos()
        {
            return _produtos;
        }
        public void AtualizarProduto(Produto produto)
        {
            var index = _produtos.FindIndex(p => p.Id == produto.Id);
            if (index == -1)
                throw new KeyNotFoundException("O Produto não foi Encontrado para a Atualização");
            _produtos[index] = produto;
        }
        public void DeletarProduto(Produto produto)
        {
            _produtos.Remove(produto);
        }
    }
}
