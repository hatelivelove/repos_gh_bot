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
    class UnsubscribeAll : Command
    {
        public string Name => "/unsubscribeall";

        public bool Contains(Message message)
        {
            return message.Text.Contains(Name);
        }

        public async Task Execute(Message message, ITelegramBotClient client)
        {
            User user = null;
            using (MyDbContext ct = new MyDbContext())
            {
                user = await ct.Users.Include(o => o.Repositories).FirstOrDefaultAsync(x => x.ChatId == message.Chat.Id);
                if (user == null)
                {
                    user = new User()
                    {
                        ChatId = message.Chat.Id,
                        Repositories = new List<Repository>()
                    };
                }
                else
                {
                    ct.Users.Remove(user);
                    await ct.SaveChangesAsync();
                }
                await Command.SendMessage("List is cleared!", message, client);
            }
        }
    }
}
