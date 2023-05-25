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
            //KeyboardService keyboardService = new KeyboardService();
            //keyboardService.Save();


            Settings settings = new Settings();
            KeyboardService keyboardService = new KeyboardService();

            BotService service = new BotService("5853125294:AAGEFhrYApRODxpPCFwjSFUQMogXQhq7Ic0", keyboardService);
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