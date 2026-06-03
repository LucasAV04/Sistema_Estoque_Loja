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
                    produto_id  AS ProdutoId,
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
                VALUES (@ProdutoId, @Quantidade);

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
                WHERE produto_id = @ProdutoId;
            ";

            connection.Execute(sql, estoque);
        }
        public IEnumerable<EstoqueDetalhado> ListarEstoqueDetalhado()
        {
            using var connection = _connectionFactory.Create();

            var sql = @"
         SELECT 
            p.id AS ProdutoId,
            p.ref AS Ref,
            p.nome AS Nome,
            p.tipo AS Tipo,
            COALESCE(e.quantidade, 0) AS Quantidade,
            p.valor_venda AS ValorVenda,
            COALESCE(e.quantidade, 0) * p.valor_venda AS ValorTotal
        FROM produto p
        LEFT JOIN estoque e ON e.produto_id = p.id
        ORDER BY p.nome";

            return connection.Query<EstoqueDetalhado>(sql);
        }
    }
}
