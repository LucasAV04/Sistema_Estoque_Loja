namespace EstoqueLoja.WPF.DTOs
{
    public class VendaResponseDto
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }
        public List<VendaItemResponseDto> Itens { get; set; } = new();
    }

    public class VendaItemResponseDto
    {
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public string RefProduto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
    }

}
