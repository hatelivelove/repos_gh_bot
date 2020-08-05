using System;
using System.Data.SqlClient;
using System.Text;

namespace MyTelegramBOT
{
    public class DbHelper
    {
        static string connectionString = "Server=tcp:repobot.database.windows.net,1433;Initial Catalog=dbTelegramBot;Persist Security Info=False;User ID=lovediehate;Password=CampFord27;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private static SqlConnectionStringBuilder build = new SqlConnectionStringBuilder() 
        {
            DataSource = connectionString,
            UserID = "lovediehate",
            Password = "CampFord27",
            InitialCatalog = "dbTelegramBot"
        };
        
        public void AddUser(long chatId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(build.ConnectionString))
                {
                    
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"INSERT INTO [USERS] (CHAT_ID) VALUES ({chatId})");

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        public  int GetUser(long chatId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(build.ConnectionString))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT (ID) FROM USERS\n");
                    sb.Append($"WHERE (CHAT_ID) = {chatId}");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            
                                return reader.GetInt32(0);
                                
                            
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return -1;
            }
        }
        public void AddRepo(string url)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(build.ConnectionString)) 
                { 
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"INSERT INTO [REPOSITORY] (URL) VALUES ('{url}')");

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        public int GetRepo(string url)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(build.ConnectionString))
                {
                    
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT (ID) FROM REPOSITORY\n");
                    sb.Append($"WHERE (URL) = '{url}'");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();

                            return reader.GetInt32(0);
                           

                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return -1;
            }
        }
       
        public void AddSub(int chatid, int repoid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(build.ConnectionString))
                {
                    
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"INSERT INTO [SUBSCRIBE] (USERID, REPOSITORYID) VALUES ({chatid}, {repoid})");

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
