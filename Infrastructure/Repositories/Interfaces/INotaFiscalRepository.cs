namespace Infrastructure.Repositories.Interfaces
{
    public interface INotaFiscalRepository
    {
        int Inserir(NotaFiscal nota);
        NotaFiscal? BuscarPorId(int id);
        NotaFiscal? BuscarPorVendaId(int vendaId);
        IEnumerable<NotaFiscal> ListarTodos();
        void Deletar(int id);
        string ProximoNumero();
    }
}
