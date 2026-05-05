using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.Memory
{
    public class MovimentacaoEstoqueMemory : IMovimentacaoEstoqueRepository
    {
        private readonly List<MovimentacaoEstoque> _mov = new();
        private int _proximoId = 1;

        public void Inserir(MovimentacaoEstoque mov)
        {
            mov.Id = _proximoId++;
            _mov.Add(mov);
        }

        public MovimentacaoEstoque? BuscarPorId(int id)
        {
            return _mov.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<MovimentacaoEstoque> ListarPorProduto(int produtoId)
        {
            return _mov
                .Where(m => m.ProdutoId == produtoId)
                .OrderByDescending(m => m.Created_At);
        }

        public IEnumerable<MovimentacaoEstoque> ListarTodos()
        {
            return _mov.OrderByDescending(m => m.Created_At);
        }
    }
}