
namespace Application.DTOs
{
    public class MovimentacaoEstoqueResponseDto
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string Tipo { get; set; }
        public int Quantidade { get; set; }
        public string Origem { get; set; }
        public string? Observacao { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
