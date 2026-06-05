using Domain;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IVendaRepository
    {
        int Inserir(Venda venda);
        void InserirItem(VendaItem item);
        Venda? BuscarPorId(int id);
        IEnumerable<Venda> ListarTodos();
    }
}
