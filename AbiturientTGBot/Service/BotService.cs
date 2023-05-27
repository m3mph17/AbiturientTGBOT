using AbiturientTGBot.AppSettings;
using AbiturientTGBot.Bot_QA;
using Newtonsoft.Json;
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
        public Answers Answers { get; private set; }

        // Services
        DBService db;
        KeyboardService keyboard;


        public BotService(BotSettings botSettings, KeyboardService keyboardService, DBService dBService)
        {
            this.Cts = new CancellationTokenSource();
            this.CancellationToken = Cts.Token;
            this.ReceiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };

            BotToken = botSettings.Token;
            Answers = botSettings.Answers;
            db = dBService;

            keyboard = keyboardService;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
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
                    // Sets to user start state
                    db.SetUserState(userId, "newUser");

                    //ReplyKeyboardMarkup keyboard = msg.CreateKeyboard(keys);

                    await botClient.SendTextMessageAsync(message.Chat,
                        text: Answers.StartMessage,
                        replyMarkup: keyboard.ClassKeyboard);

                    return;
                }

                string userState = db.GetUserState(userId);
                string newUserState = string.Empty;

                switch (userState)
                {
                    case "newUser":
                        {
                            string messageText = string.Empty;
                            
                            // Creating reply keyboard with one button cuz there's no empty constructror    
                            ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(new[] { new KeyboardButton("temp") }); 

                            if (message.Text.ToLower() == "после 9-ого")
                            {
                                messageText = Answers.SpecSuggest;
                                replyKeyboard = keyboard.BaseSpecKeyboard;
                                newUserState = "After9";
                            }

                            if (message.Text.ToLower() == "после 11-ого")
                            {
                                messageText = Answers.SpecSuggest;
                                replyKeyboard = keyboard.MidSpecKeyboard;
                                newUserState = "After11";
                            }

                            if (message.Text.ToLower() == "показать все специальности")
                            {
                                messageText = Answers.SpecSuggest;
                                replyKeyboard = keyboard.SpecialityKeyboard;
                                newUserState = "AllSpec";
                            }

                            if (messageText == "")
                                return;

                            await botClient.SendTextMessageAsync(message.Chat,
                                text: messageText,
                                replyMarkup: replyKeyboard);

                            db.SetUserState(userId, newUserState);
                        }
                        break;

                    case "After9":
                        {
                            string specInfo = string.Empty;
                            try
                            {
                                specInfo = db.GetSpecInfo(message.Text, 9);
                            }
                            catch (Exception ex)
                            {
                                specInfo = Answers.NoSpecFound;
                            }

                            await botClient.SendTextMessageAsync(message.Chat,
                                 text: specInfo);
                            
                        }
                        break;

                    case "After11":
                        {
                            string specInfo = string.Empty;
                            try
                            {
                                specInfo = db.GetSpecInfo(message.Text, 11);
                            }
                            catch (Exception ex)
                            {
                                specInfo = Answers.NoSpecFound;
                            }

                            await botClient.SendTextMessageAsync(message.Chat,
                                 text: specInfo);
                        }
                        break;

                    case "AllSpec":
                        {
                            string specInfo = string.Empty;
                            try
                            {
                                specInfo = db.GetSpecInfo(update.Message.Text);
                            }
                            catch (Exception ex)
                            {
                                specInfo = Answers.NoSpecFound;
                            }

                            await botClient.SendTextMessageAsync(message.Chat,
                                 text: specInfo);
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
