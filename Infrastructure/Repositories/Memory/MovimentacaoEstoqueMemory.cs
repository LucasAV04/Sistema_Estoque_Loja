using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.Memory
{
    public class MovimentacaoEstoqueMemory:IMovimentacaoEstoque
    {
        private readonly List<MovimentacaoEstoque> _mov = new();
        private int proximoId = 1;

        public void Inserir(MovimentacaoEstoque mov)
        {
            _mov.Add(mov);
            mov.Id = proximoId++;
        }
        public MovimentacaoEstoque BuscarPorId(int id)
        {
            return _mov.FirstOrDefault(m => m.Id == id);
        }
        public IEnumerable<MovimentacaoEstoque> ListarPorProduto(int produtoId)
        {
            return _mov
                 .Where(m => m.Produto_Id == produtoId)
                 .OrderByDescending(m => m.Created_At);
        }
        public IEnumerable<MovimentacaoEstoque> ListarTodos()
        {
            return _mov.OrderByDescending(m => m.Created_At);
        }
    }
}
