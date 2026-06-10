using Dapper;
using Domain;
using Infrastructure.Data.Connection;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.MySql
{
    public class NotaFiscalMySqlRepository:INotaFiscalRepository
    {
        private readonly MySqlConnectionFactory _factory;
        public NotaFiscalMySqlRepository(MySqlConnectionFactory factory) => _factory = factory;

        public int Inserir(NotaFiscal nota)
        {
            using var conn = _factory.Create();
            var sql = @"
                INSERT INTO nota_fiscal
                    (venda_id, numero, serie, data_emissao, nome_cliente, cpf_cnpj_cliente,
                     endereco_cliente, bairro_cliente, municipio_cliente, uf_cliente,
                     cep_cliente, telefone_cliente, natureza_operacao, forma_pagamento,
                     desconto, valor_total, vendedor, observacoes)
                VALUES
                    (@VendaId, @Numero, @Serie, @DataEmissao, @NomeCliente, @CpfCnpjCliente,
                     @EnderecoCliente, @BairroCliente, @MunicipioCliente, @UfCliente,
                     @CepCliente, @TelefoneCliente, @NaturezaOperacao, @FormaPagamento,
                     @Desconto, @ValorTotal, @Vendedor, @Observacoes);
                SELECT LAST_INSERT_ID();";
            return conn.ExecuteScalar<int>(sql, nota);
        }

        public NotaFiscal? BuscarPorId(int id)
        {
            using var conn = _factory.Create();
            return conn.QueryFirstOrDefault<NotaFiscal>(
                "SELECT * FROM nota_fiscal WHERE id = @id", new { id });
        }

        public NotaFiscal? BuscarPorVendaId(int vendaId)
        {
            using var conn = _factory.Create();
            return conn.QueryFirstOrDefault<NotaFiscal>(
                "SELECT * FROM nota_fiscal WHERE venda_id = @vendaId", new { vendaId });
        }

        public IEnumerable<NotaFiscal> ListarTodos()
        {
            using var conn = _factory.Create();
            return conn.Query<NotaFiscal>(
                "SELECT * FROM nota_fiscal ORDER BY data_emissao DESC");
        }

        public void Deletar(int id)
        {
            using var conn = _factory.Create();
            conn.Execute("DELETE FROM nota_fiscal WHERE id = @id", new { id });
        }

        public string ProximoNumero()
        {
            using var conn = _factory.Create();
            var total = conn.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM nota_fiscal");
            return (total + 1).ToString("D9");
        }
    }
}
