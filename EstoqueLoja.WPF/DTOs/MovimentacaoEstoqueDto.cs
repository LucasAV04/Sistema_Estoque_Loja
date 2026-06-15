namespace EstoqueLoja.WPF.DTOs
{
    public class MovimentacaoEstoqueDto
    {
        public int ProdutoId { get; set; }
        public string Tipo { get; set; } = "ENTRADA";
        public int Quantidade { get; set; }
        public string Origem { get; set; } = string.Empty;
        public string? Observacao { get; set; }
    }
}
