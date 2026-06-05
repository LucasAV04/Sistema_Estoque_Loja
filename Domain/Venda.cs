namespace Domain
{
    public class Venda
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }
        public List<VendaItem> Itens { get; set; } = new();
    }
}
