using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;

namespace AbiturientTGBot.Service
{
    public class MessageService
    {
        public ReplyKeyboardMarkup CreateKeyboard(string[] keysText)
        {
            KeyboardButton[] buttons = new KeyboardButton[keysText.Length];
            for (int i = 0; i < keysText.Length; i++)
            {
                buttons[i] = new KeyboardButton(keysText[i]);
            }

            var keyboard = new ReplyKeyboardMarkup(
                new[]
                {
                    buttons
                });

            keyboard.ResizeKeyboard = true;

            return keyboard;
        }
    }
}
