using Dapper;
using Domain;
using Infrastructure.Data.Connection;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.MySql
{
    public class VendaMySqlRepository:IVendaRepository
    {
        private readonly MySqlConnectionFactory _connectionFactory;

        public VendaMySqlRepository(MySqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int Inserir(Venda venda)
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                INSERT INTO venda (data, valor_total)
                VALUES (@Data, @ValorTotal);
                SELECT LAST_INSERT_ID();
            ";

            return connection.ExecuteScalar<int>(sql, new
            {
                venda.Data,
                venda.ValorTotal
            });
        }

        public void InserirItem(VendaItem item)
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                INSERT INTO venda_item
                    (venda_id, produto_id, nome_produto, ref_produto, quantidade, valor_unitario)
                VALUES
                    (@VendaId, @ProdutoId, @NomeProduto, @RefProduto, @Quantidade, @ValorUnitario);
            ";

            connection.Execute(sql, item);
        }

        public Venda? BuscarPorId(int id)
        {
            using var connection = _connectionFactory.Create();

            var venda = connection.QueryFirstOrDefault<Venda>(
                "SELECT id AS Id, data AS Data, valor_total AS ValorTotal FROM venda WHERE id = @id",
                new { id });

            if (venda == null) return null;

            venda.Itens = connection.Query<VendaItem>(@"
                SELECT
                    id              AS Id,
                    venda_id        AS VendaId,
                    produto_id      AS ProdutoId,
                    nome_produto    AS NomeProduto,
                    ref_produto     AS RefProduto,
                    quantidade      AS Quantidade,
                    valor_unitario  AS ValorUnitario
                FROM venda_item
                WHERE venda_id = @id",
                new { id }).ToList();

            return venda;
        }

        public IEnumerable<Venda> ListarTodos()
        {
            using var connection = _connectionFactory.Create();

            var vendas = connection.Query<Venda>(
                "SELECT id AS Id, data AS Data, valor_total AS ValorTotal FROM venda ORDER BY data DESC"
            ).ToList();

            if (!vendas.Any()) return vendas;

            var ids = vendas.Select(v => v.Id).ToList();

            var itens = connection.Query<VendaItem>(@"
                SELECT
                    id              AS Id,
                    venda_id        AS VendaId,
                    produto_id      AS ProdutoId,
                    nome_produto    AS NomeProduto,
                    ref_produto     AS RefProduto,
                    quantidade      AS Quantidade,
                    valor_unitario  AS ValorUnitario
                FROM venda_item
                WHERE venda_id IN @ids",
                new { ids }).ToList();

            foreach (var venda in vendas)
                venda.Itens = itens.Where(i => i.VendaId == venda.Id).ToList();

            return vendas;
        }
    }
}
