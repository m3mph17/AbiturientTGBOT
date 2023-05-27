using Telegram.Bot;
using AbiturientTGBot.Service;
using AbiturientTGBot.Models;
using AbiturientTGBot.AppSettings;

namespace AbiturientTGBot
{
    static class Program
    {
        static void Main(string[] args)
        {
            BotSettings botSettings = new BotSettings();
            botSettings.LoadSettings();

            KeyboardService keyboardService = new KeyboardService();

            BotService service = new BotService(botSettings.Token, keyboardService);
            ITelegramBotClient bot = service.CreateBot();

            bot.StartReceiving(
                service.HandleUpdateAsync,
                service.HandleErrorAsync,
                service.ReceiverOptions,
                service.CancellationToken
            );

            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            Console.ReadLine();
        }
    }
}