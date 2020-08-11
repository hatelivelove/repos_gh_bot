using Microsoft.EntityFrameworkCore;
using MyTelegramBOT.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyTelegramBOT.Model.Commands
{
    public class List : Command
    {
        public  string Name => "/list";

        public bool Contains(Message message)
        {
            return message.Text.Contains(Name);
        }

        public  async Task Execute(Message message, ITelegramBotClient client)
        {
            using (MyDbContext ct = new MyDbContext())
            {
                int count = 0;
                var user = await ct.Users.Include(o=>o.Repositories).FirstOrDefaultAsync(x => x.ChatId == message.Chat.Id);
                if(user == null)
                {
                    user = new User()
                    {
                        ChatId = message.Chat.Id,
                        Repositories = new List<Repository>()
                    };
                    ct.Users.Add(user);
                    await ct.SaveChangesAsync();
                }
                if (user.Repositories.Count == 0)
                {
                    await Command.SendMessage("List is empty!", message, client);
                    return;
                }
                foreach (var rep in user.Repositories)
                {
                    
                    count++;
                    await Command.SendMessage($"{count}-й" + rep.Url, message, client);
                }
            }   
        }
    }
}
