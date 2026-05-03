using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.Memory
{
    public class EstoqueRepositoryMemory:IEstoqueRepository
    {
        private readonly List<Estoque> _estoques = new();

        public Estoque ObterPorProdutoId(int produtoId)
        {
            return _estoques.FirstOrDefault(e => e.Produto_Id == produtoId);
        }

        public void Inserir(Estoque estoque)
        {
            _estoques.Add(estoque);
        }

        public void Atualizar(Estoque estoque)
        {
            var existente = ObterPorProdutoId(estoque.Produto_Id);

            if (existente != null)
            {
                existente.Quantidade = estoque.Quantidade;
            }
        }
    }
}
