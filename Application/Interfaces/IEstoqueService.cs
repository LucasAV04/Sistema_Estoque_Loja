using Application.DTOs;

namespace Application.Interfaces
{
    public interface IEstoqueService
    {
        void Entrada(EntradaEstoqueDto dto);
        void Saida(SaidaEstoqueDto dto);
        EstoqueResponseDto ObterPorProduto(int produtoId);
    }
}
