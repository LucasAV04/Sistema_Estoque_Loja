namespace Application.DTOs
{
    public class MovimentacaoEstoqueDto
    {
        public int ProdutoId { get; set; }
        public string Tipo { get; set; }
        public int Quantidade { get; set; }
        public string Origem { get; set; }
        public string? Observacao { get; set; }
    }
}
