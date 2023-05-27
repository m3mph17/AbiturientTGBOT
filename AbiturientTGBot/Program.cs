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
            BotSettings botSettings = new BotSettings(
                cfgPath: @"Settings\Bot.cfg",
                answersPath: @"Settings\QA\Answers.json",
                appQuestionsPath: @"Settings\QA\Application_questions.json");

            botSettings.LoadSettings();

            KeyboardService keyboardService = new KeyboardService();
            DBService dbService = new DBService();

            BotService botService = new BotService(botSettings, keyboardService, dbService);

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