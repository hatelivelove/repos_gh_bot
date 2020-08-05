using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace MyTelegramBOT
{
    class Program
    {
        private static ITelegramBotClient botClient;

        static void Main()
        {
            botClient = new TelegramBotClient(BotConfig.Token);
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
        private static long id { get; set; }
        static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                id = e.Message.Chat.Id;
                var user = DatabaseWrapper.ExecuteReader(DatabaseHelper.SelectUser(id));
                var message = e.Message.Text;
                Console.WriteLine($"Received a text message in chat {id}.");

                if (user.Count == 0)
                {
                    DatabaseWrapper.ExecuteNonQuery(DatabaseHelper.InsertUser(id));
                }
                if (message == "/info")
                {
                    Commands.Info(id);
                }
                else if (message.Contains("/subscribe"))
                {
                    Commands.Subscribe(id, message);
                }
                else if (message.Contains("/unsubscribe ") && message.Split(' ')[0] == "/unsubscribe")
                {
                    Commands.Unsubscribe(id, message);
                }
                else if(message == "/unsubscribeall")
                {
                    Commands.UnsubscribeAll(id);
                }
                else if (message == "/list")
                {
                    Commands.List(id, message);
                }
                else if(message == "/news")
                {
                    Commands.News(id);
                }
                else
                {
                    SendMessage("Unknown command. Enter /info for more information.");
                }
            }
        }
        public static async void SendMessage (string message)
        {
            await botClient.SendTextMessageAsync(
                                    chatId: id,
                                    text: message
                                  );
        }
    }
}
