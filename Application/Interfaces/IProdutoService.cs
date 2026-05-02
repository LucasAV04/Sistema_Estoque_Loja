using Application.DTOs;

namespace Application.Interfaces
{
    public interface IProdutoService
    {
        void CriarProduto(ProdutoDto dto);
        ProdutoDto BuscarPorId(int id);
        ProdutoDto BuscarPorRef(string Ref);
        ProdutoDto BuscarPorNome(string nome);
        List<ProdutoDto> ListarTodos();
        void AtualizarProduto(ProdutoDto dto);
        void DeletarProduto(ProdutoDto dto);
    }
}
