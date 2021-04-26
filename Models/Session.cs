using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCA.Models
{
    public class Session : Data
    {
        public static string CreateSession(string username)
        {
            string sessionid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string str = @"UPDATE UserInfo set SessionID='" + sessionid + "' WHERE UserName='" + username + "'";
                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.ExecuteNonQuery();
            }
            return sessionid;
        }

        public static void RemoveSession(string sessionid)
        {
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string str = @"UPDATE UserInfo set SessionID=NULL WHERE SessionID ='" + sessionid + "'";
                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static bool IsActiveSessionId(string sessionid)
        {
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string str = @"SELECT COUNT(*) FROM UserInfo WHERE SessionID = '" + sessionid + "'";
                SqlCommand cmd = new SqlCommand(str, conn);
                int count = (int)cmd.ExecuteScalar();
                return (count == 1);
            }
        }
    }
}
