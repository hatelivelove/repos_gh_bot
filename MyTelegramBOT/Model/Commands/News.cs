using Microsoft.EntityFrameworkCore;
using MyTelegramBOT.Model.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyTelegramBOT.Model.Commands
{
    public class News : Command
    {
        public  string Name => "/news";

        public bool Contains(Message message)
        {
            return message.Text.Contains(Name);
        }

        public  async Task Execute(Message message, ITelegramBotClient client)
        {
            using (MyDbContext ct = new MyDbContext())
            {
                var user = await ct.Users.FirstOrDefaultAsync(x => x.ChatId == message.Chat.Id);
                if (user == null)
                {
                    user = new User()
                    {
                        ChatId = message.Chat.Id
                    };
                    ct.Users.Add(user);
                    await ct.SaveChangesAsync();
                }
                foreach (var rep in user.Repositories)
                {
                    int count = 1;
                    string news = string.Empty;
                    
                    var username = rep.Url.Split("github.com/")[1].Split("/")[0];
                    var reponame = rep.Url.Split("github.com/")[1].Split("/")[1];
                    var date = DateTime.Now.AddDays(-1).ToString("s") + "Z";
                    string link = $"https://api.github.com/repos/{username}/{reponame}/commits?since={date}";

                    using (var webClient = new WebClient())
                    {
                        webClient.Headers.Add("User-Agent: Other");
                        var response = webClient.DownloadString(link);
                        dynamic DynamicData = JsonConvert.DeserializeObject(response);

                        news += "\nRepository " + rep + ":";
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
                        await Command.SendMessage(news, message, client);
                    }
                }
            }
        }
    }
}
