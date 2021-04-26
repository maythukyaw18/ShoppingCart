using ShoppingCartCA.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCA.Models

{
    public class UserData : Data
    {
        public static UserInfo GetUserFirstLastName(string userName)
        {
            UserInfo userinfo = new UserInfo();
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string str = @"SELECT FirstName, LastName FROM UserInfo WHERE UserName='" + userName + "'";
                SqlCommand cmd = new SqlCommand(str, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    userinfo.FirstName = (string)reader["FirstName"];
                    userinfo.LastName = (string)reader["LastName"];
                }
               
            }
            return userinfo;
        }

        public static UserInfo GetUserByUsernameAndPassword(string userName, string password)
        {

            UserInfo userinfo = null;
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();

                string q1 = @"SELECT UserName, FirstName, LastName FROM UserInfo WHERE UserName = '" + userName + "' AND Password = '" + password + "'";
                SqlCommand cmd1 = new SqlCommand(q1, conn);

                SqlDataReader reader = cmd1.ExecuteReader();
                if (reader.Read())
                {
                    userinfo = new UserInfo()
                    {
                        UserName = (string)reader["UserName"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"]
                    };
                }


            }
            return userinfo;
        }
        public static UserInfo GetUserPassword(string userName)
        {

            UserInfo userinfo = null;
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();

                string q1 = @"SELECT UserName,Password FROM UserInfo WHERE UserName = '" + userName + "'";
                SqlCommand cmd1 = new SqlCommand(q1, conn);

                SqlDataReader reader = cmd1.ExecuteReader();
                if (reader.Read())
                {
                    userinfo = new UserInfo()
                    {
                        UserName=(string)reader["UserName"],
                        Password = (string)reader["Password"]
                    };
                }


            }
            return userinfo;
        }

    }
   
}
