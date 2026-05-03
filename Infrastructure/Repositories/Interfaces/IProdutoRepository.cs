using Domain;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        void InserirProduto(Produto produto);
        Produto BuscarPorId(int id);
        Produto BuscarPorRef(string Ref);
        IEnumerable<Produto> Buscar(string nome, string Ref);
        List<Produto> ListarProdutos();
        void AtualizarProduto(Produto produto);
        void DeletarProduto(Produto produto);

    }
}
