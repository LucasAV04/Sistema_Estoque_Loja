namespace Domain
{
    public class EstoqueDetalhado
    {
        public int ProdutoId { get; set; }
        public string Ref { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorVenda { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
