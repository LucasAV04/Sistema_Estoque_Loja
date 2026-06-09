namespace EstoqueLoja.WPF.DTOs
{
    public class DadosClienteDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string CpfCnpj { get; set; } = string.Empty;
        public string Rua { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string PontoReferencia { get; set; } = string.Empty;
        public string FormaPagamento { get; set; } = "À Vista";
        public decimal Entrada { get; set; }
        public decimal Desconto { get; set; }
        public string Vendedor { get; set; } = string.Empty;
        public string Observacoes { get; set; } = string.Empty;
    }
}
