using MyTelegramBOT.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyTelegramBOT.Model.Commands
{
    public class Info : Command
    {
        public  string Name { get; } = "/info";

        public  bool Contains(Message message)
        {
             return (message.Text.Contains(Name));
        }

        public  async Task Execute(Message message, ITelegramBotClient client)
        {
            string str = "/info - текстовое описание функционала бота \n" +
                       "/subscribe < repo_url > - добавление репозитория в мои подписки\n" +
                       "Например: /subscribe https://github.com/symfony/symfony" +
                       "\n/list - вывод списка репозиториев, на которые пользователь подписан\n" +
                       "/unsubscribe <repo_url> - удаление репозитория из моих подписок\n" +
                       "Например: /unsubscribe https://github.com/symfony/symfony" +
                       "\n/news - список обновлений по избранным репозиториям(коммиты в мастер) за последние сутки\n" +
                       "/recommendations - рекомендованные популярные репозитории\n" +
                       "/unsubscribeall - удаление всех репозиториев из списка подписок";
             await Command.SendMessage(str, message, client);
        }
       
    }
}
