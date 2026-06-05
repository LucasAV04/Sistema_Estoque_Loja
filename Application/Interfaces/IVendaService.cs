using Application.DTOs;

namespace Application.Interfaces
{
    public interface IVendaService
    {
        VendaResponseDto FinalizarVenda(FinalizarVendaDto dto);
        IEnumerable<VendaResponseDto> ListarTodos();
        VendaResponseDto BuscarPorId(int id);
    }
}
