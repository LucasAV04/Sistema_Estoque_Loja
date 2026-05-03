namespace Application.DTOs
{
    public class ProdutoCreateDto
    {
        public string Ref { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public string Tipo { get; set; }
        public decimal Valor_Venda { get; set; }
        public decimal? Valor_Compra { get; set; }
    }
}
