using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCA.Models
{
    public class ProductData
    {
        private static List<Product> _GetProductDetails(string input)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();

                string q = @"SELECT * FROM Product";

                if (!String.IsNullOrEmpty(input))
                {
                    q += " WHERE ProductName LIKE '%" + input + "%' OR Description LIKE '%" + input + "%'";
                }
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product()
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = (string)reader["ProductName"],
                        Description = (string)reader["Description"],
                        Price = (decimal)(reader["Price"]),
                        ProductImg = (string)reader["ProductImg"]
                    };
                    products.Add(product);
                }
            }
            return products;
        }

        public static List<Product> GetProductDetails(string input)
        {
            return ProductData._GetProductDetails(input);
        }

        public static List<Product> GetProductDetails()
        {
            return ProductData._GetProductDetails("");
        }
        public static Product GetProduct(int productId)
        {
            Product prod = null;
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();

                string q1 = @"SELECT ProductID, " +
                            "ProductName, Description, Price, ProductImg FROM Product WHERE ProductID = " + productId;
                SqlCommand cmd = new SqlCommand(q1, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod = new Product()
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = (string)reader["ProductName"],
                        Description = (string)reader["Description"],
                        Price = (decimal)reader["Price"],
                        ProductImg = (string)reader["ProductImg"]
                    };
                }
                reader.Close();
            }
            return prod;
        }
    }
}

