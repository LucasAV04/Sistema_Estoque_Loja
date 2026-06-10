using Application.DTOs;

namespace Application.Interfaces
{
    public interface INotaFiscalService
    {
        NotaFiscalResponseDto Emitir(EmitirNotaFiscalDto dto);
        IEnumerable<NotaFiscalResponseDto> ListarTodos();
        NotaFiscalResponseDto BuscarPorId(int id);
        void Deletar(int id);
    }
}
