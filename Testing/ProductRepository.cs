using Dapper;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Testing.Models;

namespace Testing
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _connection;

        public ProductRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _connection.Query<Product>("SELECT * FROM products;");
        }

        public Product GetProduct(int id)
        {       //parameterized queries help sanitize inputs and prevent SQL injection
            return _connection.QuerySingle<Product>("SELECT * FROM products WHERE PRODUCTID = @id", new { id = id });
        }
    }
}
