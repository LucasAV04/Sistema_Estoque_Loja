namespace Domain
{
    public class MovimentacaoEstoque
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public TipoMovimentacao Tipo {  get; set; }
        public int Quantidade { get; set; }
        public string Origem { get; set; }
        public string? Observacao { get; set; }
        public DateTime Created_At { get; set; }

        public enum TipoMovimentacao 
        {
            ENTRADA,
            SAIDA
        };
    }
}
