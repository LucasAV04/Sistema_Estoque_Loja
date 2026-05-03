using Domain;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IEstoqueRepository
    {
        Estoque ObterPorProdutoId(int produtoId);
        void Inserir(Estoque estoque);
        void Atualizar(Estoque estoque);
    }
}
