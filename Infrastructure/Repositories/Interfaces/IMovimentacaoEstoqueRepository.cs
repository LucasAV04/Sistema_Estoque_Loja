using Domain;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IMovimentacaoEstoqueRepository
    {
        void Inserir(MovimentacaoEstoque movimentacao);
        MovimentacaoEstoque BuscarPorId(int id);
        IEnumerable<MovimentacaoEstoque> ListarPorProduto(int produtoId);
        IEnumerable<MovimentacaoEstoque> ListarTodos();
    }
}
