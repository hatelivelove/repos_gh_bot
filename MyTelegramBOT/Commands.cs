using Newtonsoft.Json;
using System;
using System.Net;

namespace MyTelegramBOT
{
    public static class Commands
    {
        static public void Info (long id)
        {
            string  str = "/info - текстовое описание функционала бота \n" +
                        "/subscribe < repo_url > - добавление репозитория в мои подписки\n" +
                        "Например: /subscribe https://github.com/symfony/symfony/list - вывод списка репозиториев, на которые пользователь подписан\n" +
                        "/unsubscribe < repo_url > - удаление репозитория из моих подписок\n" +
                        "Например: /unsubscribe https://github.com/symfony/symfony/news - список обновлений по избранным репозиториям(коммиты в мастер) за последние сутки\n" +
                        "/recommendations - рекомендованные популярные репозитории";
            Program.SendMessage(str, id);
        }
        static public void Subscribe (long id, string message)
        {
            if (message.Split(' ').Length == 2)
            {
                string url = message.Split(' ')[1];

                if (url != string.Empty)
                {
                    if (url.Contains("github.com/"))
                    {
                        DatabaseWrapper.ExecuteNonQuery(DatabaseHelper.InsertUser(id));
                        var userId = (int)DatabaseWrapper.ExecuteReader(DatabaseHelper.SelectUser(id))[0][0];
                        var repo = DatabaseWrapper.ExecuteReader(DatabaseHelper.SelectRepository(url));
                        if (repo.Count != 0)
                        {
                            var repoId = (int)repo[0][0];
                            DatabaseWrapper.ExecuteNonQuery(DatabaseHelper.InsertSubscribe(userId, repoId));
                        }
                        else
                        {
                            DatabaseWrapper.ExecuteNonQuery(DatabaseHelper.InsertRepository(url));
                            var repoId = (int)DatabaseWrapper.ExecuteReader(DatabaseHelper.SelectRepository(url))[0][0];
                            DatabaseWrapper.ExecuteNonQuery(DatabaseHelper.InsertSubscribe(userId, repoId));
                        }
                        Program.SendMessage("Succes!", id);
                        return;
                    }
                    else
                    {
                        Program.SendMessage("URL is invalid", id);
                    }

                }
                else
                {
                    Program.SendMessage("URL is invalid", id);
                }
            }
            else
            {
                Program. SendMessage("URL missing", id);
            }
            return;
        }
        static public void Unsubscribe (long id, string message)
        {
            string url = message.Split(' ')[1];

            if (url != string.Empty)
            {
                if (url.Contains("github.com/"))
                {
                    var userId = (int)DatabaseWrapper.ExecuteReader(DatabaseHelper.SelectUser(id))[0][0];
                    var repo = DatabaseWrapper.ExecuteReader(DatabaseHelper.SelectRepository(url));
                    if (repo.Count != 0)
                    {

                        var repoId = (int)repo[0][0];
                        DatabaseWrapper.ExecuteNonQuery(DatabaseHelper.DeleteSubscribe(userId, repoId));

                    }
                    else
                    {
                        Program.SendMessage("List is empty.", id);
                        return;
                    }
                    Program.SendMessage("Succes!", id);
                    return;
                }
                else
                {
                    Program.SendMessage("URL is invalid", id);
                }
            }
            else
            {
                Program.SendMessage("URL is invalid.", id);
            }
        }
        static public void List (long id, string message)
        {
            var userid = (int)DatabaseWrapper.ExecuteReader(DatabaseHelper.SelectUser(id))[0][0];
            var listRepo = DatabaseWrapper.ExecuteReader(DatabaseHelper.SelectSubscribe(userid));

            if (listRepo.Count == 0)
            {

                Program.SendMessage("Repositories not found.", id);
                return;
            }
            foreach (var repo in listRepo)
            {
                var repoId = (int)repo[0];
                var url = (string)DatabaseWrapper.ExecuteReader(DatabaseHelper.SelectRepository(repoId))[0][0];
                Program.SendMessage(url, id);
               
            }
            
        }
        static public void News (long id)
        {
            var userid = (int)DatabaseWrapper.ExecuteReader(DatabaseHelper.SelectUser(id))[0][0];
            var listRepo = DatabaseWrapper.ExecuteReader(DatabaseHelper.SelectSubscribe(userid));
            foreach (var repo in listRepo)
            {
                
                    var repoId = (int)repo[0];
                    var url = (string)DatabaseWrapper.ExecuteReader(DatabaseHelper.SelectRepository(repoId))[0][0];
                try
                {
                    var username = url.Split("github.com/")[1].Split("/")[0];
                    var reponame = url.Split("github.com/")[1].Split("/")[1];
                    var date = DateTime.Now.AddDays(-365).ToString("s") + "Z";
                    string link = $"https://api.github.com/repos/{username}/{reponame}/commits?since={date}";
                

                using (var webClient = new WebClient())
                {

                    webClient.Headers.Add("User-Agent: Other");

                    var response = webClient.DownloadString(link);
                    dynamic DynamicData = JsonConvert.DeserializeObject(response);
                    string news = string.Empty;
                    int count = 1;
                    news += "\nRepository " + url + ":";
                    if (DynamicData.Count == 0)
                    {
                        news += "\nUpdates don't exist.";
                    }
                    else
                    {
                        foreach (var item in DynamicData)
                        {
                            news += $"\n{count} update:\n{item.url}";
                            count++;
                        }

                    }
                    Program.SendMessage(news, id);
                }
                }
                catch
                {
                    Program.SendMessage($"{url} is not repository.", id);
                }
            }
        }
    }
}
