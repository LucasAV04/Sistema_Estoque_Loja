using Application.DTOs;

namespace Application.Interfaces
{
    public class IMovimentacaoEstoqueService
    {
        void Registrar(MovimentacaoEstoqueDto dto);
        MovimentacaoEstoqueResponseDto BuscarPorId(int id);
        IEnumerable<MovimentacaoEstoqueResponseDto> ListarPorProduto(int produtoId);
        IEnumerable<MovimentacaoEstoqueResponseDto> ListarTodos();
    }
}
