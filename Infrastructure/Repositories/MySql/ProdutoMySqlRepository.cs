using Dapper;
using Domain;
using Infrastructure.Data.Connection;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.MySql
{
    public class ProdutoMySqlRepository : IProdutoRepository
    {
        private readonly MySqlConnectionFactory _connectionFactory;

        public ProdutoMySqlRepository(MySqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Produto? BuscarPorId(int id)
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                SELECT
                    id          AS Id,
                    ref         AS Ref,
                    nome        AS Nome,
                    descricao   AS Descricao,
                    tipo        AS Tipo,
                    valor_compra AS Valor_Compra,
                    valor_venda  AS Valor_Venda
                FROM produto
                WHERE id = @id;
            ";

            return connection.QueryFirstOrDefault<Produto>(sql, new { id });
        }

        public Produto? BuscarPorRef(string refProduto)
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                SELECT
                    id          AS Id,
                    ref         AS Ref,
                    nome        AS Nome,
                    descricao   AS Descricao,
                    tipo        AS Tipo,
                    valor_compra AS Valor_Compra,
                    valor_venda  AS Valor_Venda
                FROM produto
                WHERE ref = @refProduto;
            ";

            return connection.QueryFirstOrDefault<Produto>(sql, new { refProduto });
        }

        public IEnumerable<Produto> Buscar(string? nome, string? refProduto)
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                SELECT
                    id          AS Id,
                    ref         AS Ref,
                    nome        AS Nome,
                    descricao   AS Descricao,
                    tipo        AS Tipo,
                    valor_compra AS Valor_Compra,
                    valor_venda  AS Valor_Venda
                FROM produto
                WHERE
                    (@nome IS NULL OR nome LIKE CONCAT('%', @nome, '%'))
                AND
                    (@refProduto IS NULL OR ref LIKE CONCAT('%', @refProduto, '%'))
                ORDER BY nome;
            ";

            return connection.Query<Produto>(sql, new { nome, refProduto });
        }

        public List<Produto> ListarProdutos()
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                SELECT
                    id          AS Id,
                    ref         AS Ref,
                    nome        AS Nome,
                    descricao   AS Descricao,
                    tipo        AS Tipo,
                    valor_compra AS Valor_Compra,
                    valor_venda  AS Valor_Venda
                FROM produto
                ORDER BY nome;
            ";

            return connection.Query<Produto>(sql).ToList();
        }

        public void InserirProduto(Produto produto)
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                INSERT INTO produto
                    (ref, nome, descricao, tipo, valor_compra, valor_venda)
                VALUES
                    (@Ref, @Nome, @Descricao, @Tipo, @Valor_Compra, @Valor_Venda);

                SELECT LAST_INSERT_ID();
            ";

            produto.Id = connection.ExecuteScalar<int>(sql, produto);
        }

        public void AtualizarProduto(Produto produto)
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                UPDATE produto
                SET
                    ref          = @Ref,
                    nome         = @Nome,
                    descricao    = @Descricao,
                    tipo         = @Tipo,
                    valor_compra = @Valor_Compra,
                    valor_venda  = @Valor_Venda
                WHERE id = @Id;
            ";

            connection.Execute(sql, produto);
        }

        public void DeletarProduto(Produto produto)
        {
            using var connection = _connectionFactory.Create();

        
            connection.Execute(
                "DELETE FROM venda_item WHERE produto_id = @Id",
                new { produto.Id });

           
            connection.Execute(
                "DELETE FROM movimentacao_estoque WHERE produto_id = @Id",
                new { produto.Id });

          
            connection.Execute(
                "DELETE FROM estoque WHERE produto_id = @Id",
                new { produto.Id });

          
            connection.Execute(
                "DELETE FROM produto WHERE id = @Id",
                new { produto.Id });
        }
    }
}