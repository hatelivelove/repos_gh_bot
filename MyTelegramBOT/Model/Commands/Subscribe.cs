using Microsoft.EntityFrameworkCore;
using MyTelegramBOT.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyTelegramBOT.Model.Commands
{
    public class Subscribe : Command
    {
        public string Name => "/subscribe ";

        public bool Contains(Message message)
        {
            return message.Text.Contains(Name);
        }

        public  async Task Execute(Message message, ITelegramBotClient client)
        {
            bool user_exist = false;
            //TODO: logic
            using (MyDbContext ct = new MyDbContext())
            {
                try
                {
                    foreach (var item in ct.Users.Include(o => o.Repositories).ToList())
                    {
                        if (item.ChatId == message.Chat.Id)
                        {
                            if (!CheckRepos(item.Repositories, message.Text))
                            {
                                await  Command.SendMessage("The repository already added!", message, client);
                                return;
                            }
                            Repository repos = new Repository()
                            {
                                Url = message.Text.Split(' ')[1]
                            };
                            item.Repositories.Add(repos);
                            user_exist = true;

                        }
                    }
                    if (!user_exist)
                    {
                        User user = new User()
                        {
                            ChatId = message.Chat.Id
                        };
                        Repository repos = new Repository()
                        {
                            Url = message.Text.Split(' ')[1]
                        };
                        user.Repositories.Add(repos);
                        ct.Repositories.Add(repos);
                        ct.Users.Add(user);
                    }
                    await ct.SaveChangesAsync();
                }
                catch
                {
                    await Command.SendMessage("Error!", message, client);
                }
               
            }
            await Command.SendMessage("Access!", message, client);
        }
        private bool CheckRepos (List<Repository> list, string url)
        {
            if (url.Split(' ').Count() > 2)
                return false;
            if (url.Split(' ')[0] + " " != Name)
                return false;
            foreach (var repos in list)
            {
                if (repos.Url == url)
                {
                    return false;
                }
                
            }
            return true;
        }

        
    }
}
