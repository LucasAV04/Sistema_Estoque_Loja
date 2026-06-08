namespace Application.DTOs
{
    public class VendaResponseDto
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }
        public List<VendaItemResponseDto> Itens { get; set; } = new();
    }  
}
