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
            KeyboardService keyboardService = new KeyboardService();
            DBService dbService = new DBService();

            BotSettings botSettings = new BotSettings(
                cfgPath: @"Settings\Bot.cfg",
                answersPath: @"Settings\QA\Answers.json",
                appQuestionsPath: @"Settings\QA\Application_questions.json",
                keyboard: keyboardService,
                dbService: dbService);

            botSettings.LoadSettings();


            BotService botService = new BotService(botSettings);

            ITelegramBotClient bot = botService.CreateBot();

            bot.StartReceiving(
                botService.HandleUpdateAsync,
                botService.HandleErrorAsync,
                botService.ReceiverOptions,
                botService.CancellationToken
            );

            Console.WriteLine($"Бот {bot.GetMeAsync().Result.Username} запущен");

            Console.ReadLine();
        }
    }
}