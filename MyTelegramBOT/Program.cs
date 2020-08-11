using Microsoft.Extensions.Configuration;
using MyTelegramBOT.Model;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace MyTelegramBOT
{
    class Program
    {
        private static ListCommands _commands = new ListCommands();
        public async static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var startup = new Startup();
            var botClient = startup.botClient;
            foreach (var command in _commands.Get())
            {
                if (e.Message.Text.Contains(command.Name))
                {
                    await command.Execute(e.Message, botClient);
                }
            }
            botClient.StartReceiving();

        }
        static void Main()
        {
            var startup = new Startup();
            var botClient = startup.botClient;
            var me = botClient.GetMeAsync().Result;

            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            botClient.StopReceiving();
        }
       
       
    }
}
