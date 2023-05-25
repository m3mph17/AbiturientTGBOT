﻿using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AbiturientTGBot.Service
{
    public class BotService
    {
        public string BotToken { get; private set; }
        public CancellationTokenSource Cts { get; private set; }
        public CancellationToken CancellationToken { get; private set; }
        public ReceiverOptions ReceiverOptions { get; private set; }
        public KeyboardService KeyboardService { get; private set; }

        // Db functions
        DBService db = new DBService();
        public BotService(string botToken, KeyboardService keyboardService)
        {
            this.Cts = new CancellationTokenSource();
            this.CancellationToken = Cts.Token;
            this.ReceiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };

            BotToken = botToken;

            KeyboardService = keyboardService;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия

            // If message from bot exit function
            if (update.Message.From.IsBot == true)
                return;

            Console.WriteLine(JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                long userId = message.From.Id;

                if (message.Text.ToLower() == "/start")
                {
                    // Method checks if its new user and adds it to data base
                    db.InsertNewUser(userId, message.From.Username);

                    //ReplyKeyboardMarkup keyboard = msg.CreateKeyboard(keys);

                    await botClient.SendTextMessageAsync(message.Chat,
                        text: "Привет, выбери после скольки классов планируешь поступать!",
                        replyMarkup: KeyboardService.ClassKeyboard);

                    return;
                }

                int userState = db.GetUserState(userId);

                switch (userState)
                {
                    case 0:
                        {
                            if (message.Text.ToLower() == "после 9-ого")
                            {
                                await botClient.SendTextMessageAsync(message.Chat,
                                    text: "Я могу тебе предложить следующие специальности: ",
                                    replyMarkup: KeyboardService.BaseSpecKeyboard);
                            }

                            if (message.Text.ToLower() == "после 11-ого")
                            {

                            }

                            if (message.Text.ToLower() == "показать все специальности")
                            {
                                await botClient.SendTextMessageAsync(message.Chat,
                                    text: "Я могу тебе предложить следующие специальности: ",
                                    replyMarkup: KeyboardService.SpecialityKeyboard);
                            }
                        }
                        break;
                }
            }
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(JsonConvert.SerializeObject(exception));
        }

        public ITelegramBotClient CreateBot()
        {
            ITelegramBotClient bot = new TelegramBotClient(BotToken);
            return bot;
        }
    }
}
