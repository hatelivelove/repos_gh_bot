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
    public class Unsubscribe : Command
    {
        public  string Name => "/unsubscribe ";

        public bool Contains(Message message)
        {
            return message.Text.Contains(Name);
        }

        public  async Task Execute(Message message, ITelegramBotClient client)
        {
            User user = null;
            using (MyDbContext ct = new MyDbContext())
            {
                foreach (var item in ct.Users.Include(o=>o.Repositories).ToList())
                {
                    if (item.ChatId == message.Chat.Id)
                    {
                        foreach (var rep in item.Repositories)
                        {
                            if (rep.Url == message.Text.Split(' ')[1])
                            {
                                item.Repositories.Remove(rep);
                                ct.Repositories.Remove(rep);
                                await Command.SendMessage("Succes!", message, client);
                                await ct.SaveChangesAsync();
                                return;
                            }
                        }
                        
                    }
                }
                if (user == null)
                {
                    user = new User()
                    {
                        ChatId = message.Chat.Id
                    };
                    ct.Users.Add(user);
                    await ct.SaveChangesAsync();
                    await Command.SendMessage("List is empty!", message, client);
                }
            }
            
        }
    }
}
