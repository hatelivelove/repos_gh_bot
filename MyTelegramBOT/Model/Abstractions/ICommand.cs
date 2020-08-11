using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyTelegramBOT.Model.Abstractions
{
    public interface Command
    {
        public abstract string Name { get; }
        public abstract Task Execute(Message message, ITelegramBotClient client);

        public abstract bool Contains(Message message);
        
        public static async Task SendMessage(string messageToSend, Message receivedMessage, ITelegramBotClient client)
        {
            var chatId = receivedMessage.Chat.Id;
            await client.SendTextMessageAsync(chatId, messageToSend);
        }
        
    }
}
