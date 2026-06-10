namespace EstoqueLoja.WPF.DTOs
{
    public class EmitirNotaFiscalDto
    {
        public int VendaId { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string CpfCnpjCliente { get; set; } = string.Empty;
        public string EnderecoCliente { get; set; } = string.Empty;
        public string BairroCliente { get; set; } = string.Empty;
        public string MunicipioCliente { get; set; } = string.Empty;
        public string UfCliente { get; set; } = string.Empty;
        public string CepCliente { get; set; } = string.Empty;
        public string TelefoneCliente { get; set; } = string.Empty;
        public string NaturezaOperacao { get; set; } = "VENDA DE MERCADORIAS";
        public string FormaPagamento { get; set; } = "À Vista";
        public decimal Desconto { get; set; }
        public string Vendedor { get; set; } = string.Empty;
        public string Observacoes { get; set; } = string.Empty;
    }

    public class NotaFiscalResponseDto
    {
        public int Id { get; set; }
        public int VendaId { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string Serie { get; set; } = string.Empty;
        public DateTime DataEmissao { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string CpfCnpjCliente { get; set; } = string.Empty;
        public string EnderecoCliente { get; set; } = string.Empty;
        public string BairroCliente { get; set; } = string.Empty;
        public string MunicipioCliente { get; set; } = string.Empty;
        public string UfCliente { get; set; } = string.Empty;
        public string CepCliente { get; set; } = string.Empty;
        public string TelefoneCliente { get; set; } = string.Empty;
        public string NaturezaOperacao { get; set; } = string.Empty;
        public string FormaPagamento { get; set; } = string.Empty;
        public decimal Desconto { get; set; }
        public decimal ValorTotal { get; set; }
        public string Vendedor { get; set; } = string.Empty;
        public string Observacoes { get; set; } = string.Empty;
    }
}
