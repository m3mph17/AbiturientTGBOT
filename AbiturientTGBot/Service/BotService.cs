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
                    db.SetUserState(userId, "newUser");

                    //ReplyKeyboardMarkup keyboard = msg.CreateKeyboard(keys);

                    await botClient.SendTextMessageAsync(message.Chat,
                        text: "Привет, выбери после скольки классов планируешь поступать!",
                        replyMarkup: KeyboardService.ClassKeyboard);

                    return;
                }

                string userState = db.GetUserState(userId);
                string newUserState;

                switch (userState)
                {
                    case "newUser":
                        {
                            if (message.Text.ToLower() == "после 9-ого")
                            {
                                await botClient.SendTextMessageAsync(message.Chat,
                                    text: "Я могу тебе предложить следующие специальности: ",
                                    replyMarkup: KeyboardService.BaseSpecKeyboard);

                                newUserState = "After9";
                                db.SetUserState(userId, newUserState);
                            }

                            if (message.Text.ToLower() == "после 11-ого")
                            {
                                await botClient.SendTextMessageAsync(message.Chat,
                                    text: "Я могу тебе предложить следующие специальности: ",
                                    replyMarkup: KeyboardService.MidSpecKeyboard);

                                newUserState = "After11";
                                db.SetUserState(userId, newUserState);
                            }

                            if (message.Text.ToLower() == "показать все специальности")
                            {
                                await botClient.SendTextMessageAsync(message.Chat,
                                    text: "Я могу тебе предложить следующие специальности: ",
                                    replyMarkup: KeyboardService.SpecialityKeyboard);

                                newUserState = "AllSpec";
                                db.SetUserState(userId, newUserState);
                            }
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
                                specInfo = "Выберите интересующую вас специальность";
                            }

                            await botClient.SendTextMessageAsync(message.Chat,
                                 text: specInfo);
                            
                                 //replyMarkup: KeyboardService.SpecialityKeyboard);
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
