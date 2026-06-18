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
            var sql = @"INSERT INTO movimentacao_estoque
                            (produto_id, tipo, quantidade, origem, observacao, created_at, usuario)
                        VALUES
                            (@ProdutoId, @Tipo, @Quantidade, @Origem, @Observacao, @Created_At, @Usuario);
                            SELECT LAST_INSERT_ID();";

            movimentacao.Id = connection.ExecuteScalar<int>(sql, new
            {
                movimentacao.ProdutoId,
                Tipo = movimentacao.Tipo.ToString(),
                movimentacao.Quantidade,
                movimentacao.Origem,
                movimentacao.Observacao,
                movimentacao.Created_At,
                movimentacao.Usuario
            });
        }

        public MovimentacaoEstoque? BuscarPorId(int id)
        {
            using var connection = _connectionFactory.Create();
            return connection.QueryFirstOrDefault<MovimentacaoEstoque>(@"
        SELECT id AS Id, produto_id AS ProdutoId, tipo AS Tipo,
               quantidade AS Quantidade, origem AS Origem,
               observacao AS Observacao, created_at AS Created_At,
               usuario AS Usuario
        FROM movimentacao_estoque WHERE id = @id", new { id });
        }

        public IEnumerable<MovimentacaoEstoque> ListarPorProduto(int produtoId)
        {
            using var connection = _connectionFactory.Create();
            return connection.Query<MovimentacaoEstoque>(@"
        SELECT id AS Id, produto_id AS ProdutoId, tipo AS Tipo,
               quantidade AS Quantidade, origem AS Origem,
               observacao AS Observacao, created_at AS Created_At,
               usuario AS Usuario
        FROM movimentacao_estoque
        WHERE produto_id = @produtoId ORDER BY created_at DESC", new { produtoId });
        }

        public IEnumerable<MovimentacaoEstoque> ListarTodos()
        {
            using var connection = _connectionFactory.Create();
            return connection.Query<MovimentacaoEstoque>(@"
        SELECT id AS Id, produto_id AS ProdutoId, tipo AS Tipo,
               quantidade AS Quantidade, origem AS Origem,
               observacao AS Observacao, created_at AS Created_At,
               usuario AS Usuario
        FROM movimentacao_estoque ORDER BY created_at DESC");
        }
    }
}
