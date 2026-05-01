namespace Domain
{
    public class MovimentacaoEstoque
    {
        public int Id { get; set; }
        public int Produto_Id { get; set; }
        public TipoMovimentacao Tipo {  get; set; }
        public int Quantidade { get; set; }
        public string Origem { get; set; }
        public string Observacao { get; set; }


        public enum TipoMovimentacao 
        {
            ENTRADA,
            SAIDA
        };
    }
}
