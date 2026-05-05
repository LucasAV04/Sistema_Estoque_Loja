using Dapper;
using Domain;
using Infrastructure.Data.Connection;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.MySql
{
    public class MovimentacaoEstoqueMySqlRepository : IMovimentacaoEstoqueRepository
    {
        private readonly MySqlConnectionFactory _connectionFactory;

        public MovimentacaoEstoqueMySqlRepository(MySqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Inserir(MovimentacaoEstoque movimentacao)
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                INSERT INTO movimentacao_estoque
                    (produto_id, tipo, quantidade, origem, observacao, created_at)
                VALUES
                    (@Produto_Id, @Tipo, @Quantidade, @Origem, @Observacao, @Created_At);

                SELECT LAST_INSERT_ID();
            ";

            movimentacao.Id = connection.ExecuteScalar<int>(sql, new
            {
                movimentacao.ProdutoId,
                Tipo = movimentacao.Tipo.ToString(),
                movimentacao.Quantidade,
                movimentacao.Origem,
                movimentacao.Observacao,
                movimentacao.Created_At
            });
        }

        public MovimentacaoEstoque? BuscarPorId(int id)
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                SELECT
                    id          AS Id,
                    produto_id  AS Produto_Id,
                    tipo        AS Tipo,
                    quantidade  AS Quantidade,
                    origem      AS Origem,
                    observacao  AS Observacao,
                    created_at  AS Created_At
                FROM movimentacao_estoque
                WHERE id = @id;
            ";

            return connection.QueryFirstOrDefault<MovimentacaoEstoque>(sql, new { id });
        }

        public IEnumerable<MovimentacaoEstoque> ListarPorProduto(int produtoId)
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                SELECT
                    id          AS Id,
                    produto_id  AS Produto_Id,
                    tipo        AS Tipo,
                    quantidade  AS Quantidade,
                    origem      AS Origem,
                    observacao  AS Observacao,
                    created_at  AS Created_At
                FROM movimentacao_estoque
                WHERE produto_id = @produtoId
                ORDER BY created_at DESC;
            ";

            return connection.Query<MovimentacaoEstoque>(sql, new { produtoId });
        }

        public IEnumerable<MovimentacaoEstoque> ListarTodos()
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                SELECT
                    id          AS Id,
                    produto_id  AS Produto_Id,
                    tipo        AS Tipo,
                    quantidade  AS Quantidade,
                    origem      AS Origem,
                    observacao  AS Observacao,
                    created_at  AS Created_At
                FROM movimentacao_estoque
                ORDER BY created_at DESC;
            ";

            return connection.Query<MovimentacaoEstoque>(sql);
        }
    }
}
