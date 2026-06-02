namespace EstoqueLoja.WPF.DTOs
{
    public class ProdutoCreateDto
    {
        public string Ref { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Valor_Venda { get; set; }
        public decimal? Valor_Compra { get; set; }
    }
}
