using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCartCA.Models
{
    public class CartFunctions
    {
        public static int GetCartItemCount(string userName)
        {
            int count = 0;
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string q = @"SELECT SUM(Quantity) ItemCount FROM Cart WHERE UserName='" + userName + "'";
                SqlCommand cmd1 = new SqlCommand(q, conn);
                object result = cmd1.ExecuteScalar();
                result = (result == DBNull.Value) ? null : result;
                count = Convert.ToInt32(result);
            }
            return count;
        }


        public static List<Cart> GetCartItems(string userName)
        {
            List<Cart> carts = new List<Cart>();
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();

                string q1 = @"SELECT UserName, p.ProductID AS ProductID, Quantity, " +
                            "ProductName, Description, Price, ProductImg FROM Cart c, Product p WHERE c.UserName = '" + userName +
                            "' AND c.ProductID = p.ProductID";
                SqlCommand cmd = new SqlCommand(q1, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product prod = new Product()
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = (string)reader["ProductName"],
                        Description = (string)reader["Description"],
                        Price = (decimal)reader["Price"],
                        ProductImg = (string)reader["ProductImg"]
                    };

                    Cart cart = new Cart()
                    {
                        ProductID = (int)reader["ProductID"],
                        UserName = (string)reader["UserName"],
                        Quantity = (int)reader["Quantity"],
                        Product = prod

                    };
                    carts.Add(cart);
                }
                reader.Close();
            }
            return carts;
        }

        public static void EmptyCart(string userName)
        {
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();

                string q1 = @"DELETE from Cart where UserName = '" + userName + "'";
                SqlCommand cmd1 = new SqlCommand(q1, conn);
                cmd1.ExecuteNonQuery();
            }
        }

        public static void AddItemToCart(int productId, int qty, string userName, string sessionID)
        {
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();

                string q1 = @"SELECT COUNT(*) from Cart where ProductID = '" + productId + "' and UserName = '" + userName + "'";
                SqlCommand cmd1 = new SqlCommand(q1, conn);
                int count = (int)cmd1.ExecuteScalar();

                if (count > 0)
                {
                    string q2 = @"UPDATE Cart SET Quantity = Quantity + " + qty + " WHERE ProductID ='" + productId + "' and UserName = '" + userName + "'";
                    SqlCommand cmd2 = new SqlCommand(q2, conn);
                    cmd2.ExecuteNonQuery();
                }
                else
                {
                    string q3 = "INSERT INTO Cart (UserName,ProductID,Quantity,SessionId)" + "VALUES ('" + userName + "','" + productId + "'," + qty + ",'" + sessionID + "')";
                    SqlCommand cmd3 = new SqlCommand(q3, conn);
                    cmd3.ExecuteNonQuery();
                }
            }
        }

        public static int AddPurchaseHeader(string username)
        {
            //string purchaseId = GetSequenceNumber();
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string q = "INSERT INTO Purchase (UserName, Date) VALUES ('" + username + "', GETDATE())" +
                    " SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(q, conn);
                object result = cmd.ExecuteScalar();
                result = (result == DBNull.Value) ? null : result;
                return Convert.ToInt32(result);
            }
        }

        public static void AddPurchaseDetails(int purchaseId, int productid, string activationCode)
        {
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string q = "INSERT INTO PurchaseDetails VALUES ('" + purchaseId + "','" + productid + "','" + activationCode + "')";
                SqlCommand com = new SqlCommand(q, conn);
                com.ExecuteNonQuery();
            }
        }
        public static void RemoveFromCart(int productId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();

                
                string q1 = @"DELETE from Cart where ProductID = '" + productId + "' and UserName = '" + userName + "'";
                SqlCommand cmd1 = new SqlCommand(q1, conn);
                cmd1.ExecuteNonQuery();

            }
        }

        public static List<Cart> GetCartCountPerItem(string userName, int productId)
        {
            List<Cart> carts = new List<Cart>();
            using(SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string q2 = @"SELECT Quantity FROM Cart WHERE ProductID = '" + productId + "' and UserName = '" + userName + "'";
                SqlCommand cmd = new SqlCommand(q2, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Cart cart = new Cart()
                    {
                        Quantity = (int)reader["inititalQty"]
                    };
                    carts.Add(cart);
                }
                return carts;
                
            }
        }
        public static void UpdateQuantity(int productId, string userName, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                if (quantity > 0)
                {
                    string q2 = @"UPDATE CART SET Quantity = " + quantity + " WHERE UserName = '" + userName + "' and ProductID = " + productId;
                    SqlCommand cmd = new SqlCommand(q2, conn);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string q3 = @"DELETE FROM Cart WHERE UserName='" + userName + "' and ProductID=" + productId;
                    SqlCommand cmd = new SqlCommand(q3, conn);
                    cmd.ExecuteNonQuery();
                }
                

            }
        }
        
    }
}

