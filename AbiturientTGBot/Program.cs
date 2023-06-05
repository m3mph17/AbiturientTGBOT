using Telegram.Bot;
using AbiturientTGBot.Service;
using AbiturientTGBot.Models;
using AbiturientTGBot.AppSettings;
using AbiturientTGBot.Handlers;
using System.Runtime.InteropServices;

namespace AbiturientTGBot
{
    static class Program
    {
        static void KeyListenerThread()
        {
            [DllImport("user32.dll")]
            static extern short GetAsyncKeyState(int keyCode);

            bool keyDown = false;
            DBService dbService = new DBService();

            while (true)
            {
                short keyState = GetAsyncKeyState(0x74); // 0x74 is the virtual key code for F5
                bool keyPressed = (keyState & 0x8000) != 0;

                if (keyPressed && !keyDown)
                {
                    Console.WriteLine("Экспорт заявок в Excel начат");
                    ExportHandler export = new ExportHandler(dbService, "Exports");
                    export.ExportToExcel();

                    keyDown = true;
                }
                else if (!keyPressed && keyDown)
                {
                    keyDown = false;
                }

                // Sleep for a short amount of time to avoid using too much CPU
                Thread.Sleep(10);
            }
        }

        static void Main(string[] args)
        {
            KeyboardService keyboardService = new KeyboardService();
            DBService dbService = new DBService();

            Thread keyListenerThread = new Thread(KeyListenerThread);
            keyListenerThread.Start();


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