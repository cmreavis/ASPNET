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

        public void UpdateProduct(Product product)
        {
            _connection.Execute("UPDATE products SET Name = @name, Price = @price WHERE ProductID = @id", 
                new { name = product.Name, price = product.Price, id = product.ProductID });
        }

        public void InsertProduct(Product productToInsert)
        {
            _connection.Execute("INSERT INTO products (NAME, PRICE, CATEGORYID) VALUES (@name, @price, @categoryID);",
                new { name = productToInsert.Name, price = productToInsert.Price, categoryID = productToInsert.CategoryID });
        }

        void IProductRepository.DeleteProduct(Product product)
        {
            _connection.Execute("DELETE FROM products WHERE PRODUCTID = @id;", new { id = product.ProductID });
            _connection.Execute("DELETE FROM sales WHERE PRODUCTID = @id;", new { id = product.ProductID });
            _connection.Execute("DELETE FROM reviews WHERE PRODUCTID = @id;", new { id = product.ProductID }); 
        }

        public Product AssignCategory()
        {
            var categoryList = GetAllCategories();
            var product = new Product();
            product.Categories = categoryList;
            return product;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _connection.Query<Category>("SELECT * FROM categories;");
        }

    }
}
