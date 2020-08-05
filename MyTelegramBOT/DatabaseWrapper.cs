using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace MyTelegramBOT
{
    public static class DatabaseWrapper
    {
        private static SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
        {
            DataSource = "repobot.database.windows.net",
            UserID = "lovediehate",
            Password = "CampFord27",
            InitialCatalog = "dbTelegramBot"
        };
        public static void ExecuteNonQuery(string sql)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
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
        public static List<object[]> ExecuteReader(string sql)
        {
            var answer = new List<object[]>();

         
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            object[] objects = new object[reader.FieldCount];
                            reader.GetValues(objects);
                            answer.Add(objects);
                        }
                    }
                }
                return answer;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return answer;
            }
        }
    }
}
