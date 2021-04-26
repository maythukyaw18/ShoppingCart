using Microsoft.AspNetCore.Hosting;
using ShoppingCartCA.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCA.Models
{
    public class MyPurchaseData : Data
    {   
       
        public static List<MyPurchase> GetPurchaseHeaderByUserName(string name) 
        {
            List<MyPurchase> myPurchases = new List<MyPurchase>();
            using(SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string sql4 = @"SELECT pd.ProductId, p.PurchaseId, p.UserName, p.Date, Count(pd.ActivationCode) CountOfActivationCodes, pr.ProductImg, pr.Description,pr.ProductName 
                                FROM PurchaseDetails pd JOIN Purchase p ON p.PurchaseID = pd.PurchaseID JOIN Product pr ON pr.ProductID = pd.ProductID 
                                WHERE p.UserName = '" + name + "'GROUP BY pd.ProductId, p.PurchaseId, p.UserName, p.Date, pr.ProductImg,pr.ProductName,pr.Description";
                SqlCommand cmd = new SqlCommand(sql4, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    MyPurchase myPurchase = new MyPurchase
                    {
                        ProductID = (int)reader["productID"],
                        PurchaseID = (int)reader["purchaseID"],
                        UserName = (string)reader["username"],
                        Date = (DateTime)reader["date"],
                        ProductName = (string)reader["productName"],
                        Description = (string)reader["description"],
                        ProductImg = (string)reader["productImg"],
                        
                    };
                    myPurchases.Add(myPurchase);
                }
                return myPurchases;
            }
        }
        public static List<PurchaseDetails> GetActivationCodes(int productID, int purchaseID)
        {
            List<PurchaseDetails> purchaseDetails = new List<PurchaseDetails>();
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string sql5 = @"SELECT ActivationCode FROM PurchaseDetails pd JOIN Purchase p ON pd.PurchaseID = p.PurchaseID WHERE p.PurchaseID ='" + purchaseID + "' and pd.ProductID =" + productID + "";
                SqlCommand cmd = new SqlCommand(sql5, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PurchaseDetails details = new PurchaseDetails
                    {
                        ActivationCode = (string)reader["activationCode"]
                    };
                    purchaseDetails.Add(details);
                }
                return purchaseDetails;
            }
        }
    }

   
}
