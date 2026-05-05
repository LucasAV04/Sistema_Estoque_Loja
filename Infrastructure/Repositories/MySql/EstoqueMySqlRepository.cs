using Dapper;
using Domain;
using Infrastructure.Data.Connection;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.MySql
{
    public class EstoqueMySqlRepository : IEstoqueRepository
    {
        private readonly MySqlConnectionFactory _connectionFactory;

        public EstoqueMySqlRepository(MySqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Estoque? ObterPorProdutoId(int produtoId)
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                SELECT
                    id          AS Id,
                    produto_id  AS Produto_Id,
                    quantidade  AS Quantidade
                FROM estoque
                WHERE produto_id = @produtoId;
            ";

            return connection.QueryFirstOrDefault<Estoque>(sql, new { produtoId });
        }

        public void Inserir(Estoque estoque)
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                INSERT INTO estoque (produto_id, quantidade)
                VALUES (@Produto_Id, @Quantidade);

                SELECT LAST_INSERT_ID();
            ";

            estoque.Id = connection.ExecuteScalar<int>(sql, estoque);
        }

        public void Atualizar(Estoque estoque)
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
                UPDATE estoque
                SET quantidade = @Quantidade
                WHERE produto_id = @Produto_Id;
            ";

            connection.Execute(sql, estoque);
        }
    }
}
