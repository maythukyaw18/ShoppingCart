using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCA.Models
{
    public class ReviewFunction: Data
    {
        public static void AddProductReview(int productId, int purchaseId, int rating)
        {
            using(SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string q = @"INSERT INTO Review (ProductID, PurchaseID, Rating) VALUES ("+ productId + "," + purchaseId + "," + rating + ")";
                SqlCommand cmd = new SqlCommand(q,conn); 
                cmd.ExecuteNonQuery();
            }

        }

        public static decimal GetProductReviewAverage(int productId)
        {
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string q1 = @"SELECT AVG(Rating) FROM Review WHERE ProductID = " + productId + " GROUP BY ProductID";
                SqlCommand cmd = new SqlCommand(q1, conn);
                object productRating = cmd.ExecuteScalar();
                productRating = (productRating == DBNull.Value) ? null : productRating;
                return (productRating == null ? 0 : Decimal.Round(Convert.ToDecimal(productRating), 2));
            }

        }
        public static int GetProductReviewCount(int productId)
        {
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string q1 = @"SELECT COUNT(Rating) FROM Review WHERE ProductID = " + productId + " GROUP BY ProductID";
                SqlCommand cmd = new SqlCommand(q1, conn);
                object productRating = cmd.ExecuteScalar();
                productRating = (productRating == DBNull.Value) ? null : productRating;
                return (productRating == null ? 0 : Convert.ToInt32(productRating));
            }

        }

        public static Review GetPurchasedProduceReview(int purchaseId, int productId)
        {
            Review review = null;
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string q3 = @"SELECT ProductId, PurchaseId, Rating, CreatedAt FROM Review WHERE PurchaseID = " + purchaseId + " and ProductID = " + productId;
                SqlCommand cmd = new SqlCommand(q3, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    review = new Review()
                    {
                        ProductID = (int)reader["ProductID"],
                        PurchaseID = (int)reader["PurchaseId"],
                        Rating = (int)reader["Rating"],
                        CreatedAt = (DateTime)reader["CreatedAt"]
                    };
                }
                reader.Close();
            }
            return review;
        }
    }
}
