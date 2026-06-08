using EstoqueLoja.WPF.DTOs;

namespace EstoqueLoja.WPF.ViewModels
{
    public class VendaHistoricoViewModel
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }
        public int TotalItens { get; set; }
        public List<VendaItemResponseDto> Itens { get; set; } = new();
    }
}
