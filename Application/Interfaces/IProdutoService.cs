using Application.DTOs;


namespace Application.Interfaces
{
    public interface IProdutoService
    {
        void CriarProduto(ProdutoCreateDto dto);
        ProdutoResponseDto BuscarPorId(int id);
        ProdutoResponseDto BuscarPorRef(string Ref);
        IEnumerable<ProdutoResponseDto> Buscar(string? nome, string? Ref);
        List<ProdutoResponseDto> ListarTodos();
        void AtualizarProduto(int id,ProdutoCreateDto dto);
        void DeletarProduto(int id);
    }
}
