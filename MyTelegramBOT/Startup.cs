using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace MyTelegramBOT
{
    public class Startup
    {
        public  IConfiguration _configuration { get; set; }
        public  TelegramBotClient botClient { get; set; }

        public Startup()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            _configuration = builder.Build();
            botClient = new TelegramBotClient(_configuration.GetConnectionString("Token"));
        }
    }
}
