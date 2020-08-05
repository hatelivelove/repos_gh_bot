namespace MyTelegramBOT
{
    public static class DatabaseHelper
    {
        public static string InsertUser(long chatId)
        {
            return $"INSERT INTO [USERS] (CHAT_ID) VALUES({ chatId})";
        }
        public static string InsertRepository(string url)
        {
            return $"INSERT INTO [REPOSITORY] (URL) VALUES ('{url}')";
        }
        public static string SelectUser(long chatId)
        {
            return $"SELECT (ID) FROM [USERS] WHERE (CHAT_ID) = {chatId}";
        }
        public static string SelectRepository(string url)
        {
            return $"SELECT (ID) FROM [REPOSITORY] WHERE (URL) = '{url}'";
        }
        public static string SelectRepository(int repoID)
        {
            return $"SELECT (URL) FROM [REPOSITORY] WHERE (ID) = {repoID}";
        }
        public static string SelectSubscribe (int userId)
        {
            return $"SELECT (REPOSITORYID) FROM SUBSCRIBE WHERE USERID = {userId}";
        }
        public static string InsertSubscribe (int userId, int repoId)
        {
            return $"INSERT INTO [SUBSCRIBE] (USERID,REPOSITORYID) VALUES ({userId}, {repoId})";
        }
        public static string DeleteSubscribe (int userId, int repoId)
        {
            return $"DELETE FROM [SUBSCRIBE] WHERE USERID = {userId} AND REPOSITORYID = {repoId}";
        }
    }
}
