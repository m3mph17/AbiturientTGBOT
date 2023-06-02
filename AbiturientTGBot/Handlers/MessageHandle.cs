using AbiturientTGBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = AbiturientTGBot.Models.User;

namespace AbiturientTGBot.Handlers
{
    public class MessageHandle
    {
        public User User { get; set; }
        public string Message { get; set; }
        public ReplyKeyboardMarkup? ReplyKeyboard { get; set; }
        public InlineKeyboardMarkup? InlineKeyboard {  get; set; }
    }
}
