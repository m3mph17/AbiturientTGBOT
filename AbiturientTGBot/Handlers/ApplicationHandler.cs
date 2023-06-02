using AbiturientTGBot.AppSettings;
using AbiturientTGBot.Bot_QA;
using AbiturientTGBot.Models;
using AbiturientTGBot.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = AbiturientTGBot.Models.User;

namespace AbiturientTGBot.Handlers
{
    public class ApplicationHandler
    {
        User user;
        string? message;
        CallbackQuery callback;

        BotService bot;

        // Services
        DBService db;
        MessageService messageService = new MessageService();
        KeyboardService keyboard;
        ApplicationQuestions questions;
        Answers answers;

        public ApplicationHandler(object sender, long userId, string? message)
        {
            bot = (BotService)sender;
            db = bot.db;
            keyboard = bot.keyboard;
            user = bot.db.GetUser(userId);

            this.message = message;
            questions = bot.settings.ApplicQuestions;
            answers = bot.settings.Answers;
        }

        public ApplicationHandler(object sender, long userId, CallbackQuery callback)
        {
            bot = (BotService)sender;
            db = bot.db;
            keyboard = bot.keyboard;
            user = bot.db.GetUser(userId);

            questions = bot.settings.ApplicQuestions;
            answers = bot.settings.Answers;
            this.callback = callback;
        }

        public MessageHandle HandleCallBack()
        {
            string data = callback.Data;
            MessageHandle messageHandle = new MessageHandle { User = user };
            Abiturient abiturient = db.GetAbiturient(user.Id);

            InlineKeyboardButton[][] socialButtons = keyboard.socialButtons;

            if (data == "manyChildrenTrue" || data == "manyChildrenFalse")
            {
                InlineKeyboardButton newButton = ChangeButtonState(socialButtons[0][0]);
                socialButtons[0][0] = newButton;

                if (abiturient.IsManyChildren == false)
                    abiturient.IsManyChildren = true;

                if (abiturient.IsManyChildren == true)
                    abiturient.IsManyChildren = false;

                if (abiturient.IsManyChildren == null)
                    abiturient.IsManyChildren = true;
            }

            if (data == "orphanFalse" || data == "orphanTrue")
            {
                InlineKeyboardButton newButton = ChangeButtonState(socialButtons[1][0]);
                socialButtons[1][0] = newButton;

                if (abiturient.IsOrphan == false)
                    abiturient.IsOrphan = true;

                if (abiturient.IsOrphan == true)
                    abiturient.IsOrphan = false;

                if (abiturient.IsOrphan == null)
                    abiturient.IsOrphan = true;
            }

            if (data == "chernobylFalse" || data == "chernobylTrue")
            {
                InlineKeyboardButton newButton = ChangeButtonState(socialButtons[2][0]);
                socialButtons[2][0] = newButton;

                if (abiturient.IsFromChernobyl == false)
                    abiturient.IsFromChernobyl = true;

                if (abiturient.IsFromChernobyl == true)
                    abiturient.IsFromChernobyl = false;

                if (abiturient.IsFromChernobyl == null)
                    abiturient.IsFromChernobyl = true;
            }

            if (data == "hostelFalse" || data == "hostelTrue")
            {
                InlineKeyboardButton newButton = ChangeButtonState(socialButtons[3][0]);
                socialButtons[3][0] = newButton;

                if (abiturient.IsNeedHostel == true)
                    abiturient.IsNeedHostel = false;

                if (abiturient.IsNeedHostel == false)
                    abiturient.IsNeedHostel = true;

                if (abiturient.IsNeedHostel == null)
                    abiturient.IsNeedHostel = true;                
            }

            if (data == "opfrFalse" || data == "opfrTrue")
            {
                InlineKeyboardButton newButton = ChangeButtonState(socialButtons[4][0]);
                socialButtons[4][0] = newButton;

                if (abiturient.IsOpfr == true)
                    abiturient.IsOpfr = false;

                if (abiturient.IsOpfr == false)
                    abiturient.IsOpfr = true;

                if (abiturient.IsOpfr == null)
                    abiturient.IsOpfr = true;
                
            }

            if (data == "submitData")
            {
                string msg = 
                    "Информация подтверждена.\nНиже предоставлены ваши ответы:\n\n" + 
                    messageService.CreateSocialInfo(abiturient) +
                    "\n\nСпасибо за заполнение заявки!";

                messageHandle.Message = msg;
                messageHandle.InlineKeyboard = null;

                if (abiturient.IsManyChildren == null)
                    abiturient.IsManyChildren = false;

                if (abiturient.IsOrphan == null)
                    abiturient.IsOrphan = false;

                if (abiturient.IsFromChernobyl == null)
                    abiturient.IsFromChernobyl = false;

                if (abiturient.IsOpfr == null)
                    abiturient.IsOpfr = false;

                if (abiturient.IsNeedHostel == null)
                    abiturient.IsNeedHostel = false;

                abiturient.IsFull = true;
                db.UpdateAbiturient(abiturient);

                return messageHandle;
            }

            db.UpdateAbiturient(abiturient);
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(socialButtons);
            messageHandle.InlineKeyboard = inlineKeyboard;

            return messageHandle;
        }

        public MessageHandle Handle()
        {
            Abiturient abiturient = db.GetAbiturient(user.Id);
            MessageHandle messageHandle = new MessageHandle { User = user };

            if (abiturient == null)
            {
                abiturient = new Abiturient { UserId = user.Id };
                db.CreateNewAbiturient(abiturient);

                messageHandle.Message = questions.Specizalization;
                messageHandle.ReplyKeyboard = keyboard.SpecialityKeyboard;

                return messageHandle;
            }

            if (abiturient.IsFull == true)
            {
                messageHandle.Message = answers.ApplicationFulled;
                messageHandle.ReplyKeyboard = null;

                return messageHandle;
            }


            if (abiturient.SpecId == null)
            {
                Specialization spec;
                try
                {
                    spec = db.GetSpecizalization(message);
                }
                catch (Exception ex)
                {
                    messageHandle.Message = answers.BadSpecInput;
                    messageHandle.ReplyKeyboard = null;

                    return messageHandle;
                }

                abiturient.SpecId = spec.SpecId;
                db.UpdateAbiturient(abiturient);
                messageHandle.Message = questions.Fio;
                messageHandle.ReplyKeyboard = null;

                return messageHandle;
            }

            if (abiturient.Surname == null)
            {
                try
                {
                    string[] fio = message.Split(' ');
                    abiturient.Surname = fio[0];
                    abiturient.Firstname = fio[1];
                    abiturient.Patronymic = fio[2];

                    db.UpdateAbiturient(abiturient);

                    messageHandle.Message = questions.IsMale;
                    messageHandle.ReplyKeyboard = keyboard.IsMaleKeyboard;
                }
                catch (Exception ex)
                {
                    messageHandle.Message = answers.BadFioInput; // bad input answer;
                    messageHandle.ReplyKeyboard = null;
                }
                
                return messageHandle;
            }

            if (abiturient.IsMale == null)
            {
                if (message.ToLower() == "мужской")
                    abiturient.IsMale = true;

                if (message.ToLower() == "женский")
                    abiturient.IsMale = false;

                if (message.ToLower() != "мужской" && message.ToLower() != "женский")
                {
                    messageHandle.Message = answers.BadMaleInput;
                    messageHandle.ReplyKeyboard = keyboard.IsMaleKeyboard;

                    return messageHandle;
                }

                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.Address;
                messageHandle.ReplyKeyboard = null;

                return messageHandle;
            }

            if (abiturient.Address == null)
            {
                abiturient.Address = message;
                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.SchoolName;
                messageHandle.ReplyKeyboard = null;

                return messageHandle;
            }

            if (abiturient.SchoolName == null)
            {
                abiturient.SchoolName = message;
                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.SchoolAddress;
                messageHandle.ReplyKeyboard = null;

                return messageHandle;
            }

            if (abiturient.SchoolAddress == null)
            {
                abiturient.SchoolAddress = message;
                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.SchoolMark;
                messageHandle.ReplyKeyboard = null;

                return messageHandle;
            }

            if (abiturient.SchoolMark == null)
            {
                try
                {
                    double mark = Convert.ToDouble(message);
                    abiturient.SchoolMark = mark;

                    db.UpdateAbiturient(abiturient);

                    messageHandle.Message = questions.HomeNumber;
                    messageHandle.ReplyKeyboard = null;
                }
                catch (Exception ex)
                {
                    messageHandle.Message = answers.BadSchoolMarkInput;
                    messageHandle.ReplyKeyboard = null;

                    return messageHandle;
                }

                return messageHandle;
            }


            if (abiturient.HomePhone == null)
            {
                try
                {
                    abiturient.HomePhone = Convert.ToInt64(message);
                    db.UpdateAbiturient(abiturient);

                    messageHandle.Message = questions.PhoneNumber;
                    messageHandle.ReplyKeyboard = null;
                }
                catch (Exception ex)
                {
                    messageHandle.Message = answers.BadHomeNumberInput; // bad input;
                    messageHandle.ReplyKeyboard = null;
                }
                
                return messageHandle;
            }

            if (abiturient.MobilePhone == null)
            {
                try
                {
                    // regex pattern
                    string pattern = @"^(375|80)(29|25|44|33)\d{7}$";

                    bool isMatch = Regex.IsMatch(message, pattern);

                    if (isMatch == false)
                    {
                        throw new Exception("Bad phonenumber input");
                    }

                    abiturient.MobilePhone = Convert.ToInt64(message);
                    db.UpdateAbiturient(abiturient);

                    messageHandle.Message = questions.Birthday;
                    messageHandle.ReplyKeyboard = null;
                }
                catch (Exception ex)
                {
                    messageHandle.Message = answers.BadPhoneNumberInput; // bad input;
                    messageHandle.ReplyKeyboard = null;
                }

                return messageHandle;
            }


            if (abiturient.BirthDate == null)
            {
                string pattern = @"^([0-2]\d|3[01])\.(0[1-9]|1[0-2])\.(19|20)\d{2}$";
                bool isMatch = Regex.IsMatch(message, pattern);

                if (isMatch == false)
                {
                    messageHandle.Message = answers.BadBirthdateInput; // bad input
                    messageHandle.ReplyKeyboard = null;

                    return messageHandle;
                }

                abiturient.BirthDate = message;
                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.InvalidGroup;
                messageHandle.ReplyKeyboard = keyboard.InvalidGroupKeyboard;

                return messageHandle;
            }

            if (abiturient.InvalidGroup == null)
            {
                try
                {
                    if (message.ToLower() == "нет инвалидности")
                        abiturient.InvalidGroup = 0;
                    else
                        abiturient.InvalidGroup = Convert.ToInt32(message);
                }
                catch (Exception ex)
                {
                    messageHandle.Message = answers.BadInvalidGroupInput;
                    messageHandle.ReplyKeyboard = keyboard.InvalidGroupKeyboard;

                    return messageHandle;
                }

                //abiturient.IsFull = true;
                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.SocialStatus;
                messageHandle.InlineKeyboard = keyboard.SocialInlineKeyboard;

                //db.SetUserStateByTableId(user.Id, "newUser");

                return messageHandle;
            }

            // Change to multiply
            if (abiturient.IsOrphan == null)
            {

                    
                return messageHandle;
            }

            //if (abiturient.IsOrphan == null)
            //{
            //    if (message.ToLower() == "да")
            //        abiturient.IsOrphan = true;
            //    if (message.ToLower() == "нет")
            //        abiturient.IsOrphan = false;

            //    if (message.ToLower() != "да" && message.ToLower() != "нет")
            //    {
            //        messageHandle.Message = answers.BadYesNoInput;
            //        messageHandle.ReplyKeyboard = keyboard.YesNoKeyboard;

            //        return messageHandle;
            //    }

            //    db.UpdateAbiturient(abiturient);

            //    messageHandle.Message = questions.IsChernobyl;
            //    messageHandle.ReplyKeyboard = keyboard.YesNoKeyboard;

            //    return messageHandle;
            //}

            //if (abiturient.IsFromChernobyl == null)
            //{
            //    if (message.ToLower() == "да")
            //        abiturient.IsFromChernobyl = true;
            //    if (message.ToLower() == "нет")
            //        abiturient.IsFromChernobyl = false;

            //    if (message.ToLower() != "да" && message.ToLower() != "нет")
            //    {
            //        messageHandle.Message = answers.BadYesNoInput;
            //        messageHandle.ReplyKeyboard = keyboard.YesNoKeyboard;

            //        return messageHandle;
            //    }

            //    db.UpdateAbiturient(abiturient);

            //    messageHandle.Message = questions.IsManyChildren;
            //    messageHandle.ReplyKeyboard = keyboard.YesNoKeyboard;

            //    return messageHandle;
            //}

            //if (abiturient.IsManyChildren == null)
            //{
            //    if (message.ToLower() == "да")
            //        abiturient.IsManyChildren = true;
            //    if (message.ToLower() == "нет")
            //        abiturient.IsManyChildren = false;

            //    if (message.ToLower() != "да" && message.ToLower() != "нет")
            //    {
            //        messageHandle.Message = answers.BadYesNoInput;
            //        messageHandle.ReplyKeyboard = keyboard.YesNoKeyboard;

            //        return messageHandle;
            //    }

            //    db.UpdateAbiturient(abiturient);

            //    messageHandle.Message = questions.IsOpfr;
            //    messageHandle.ReplyKeyboard = keyboard.YesNoKeyboard;

            //    return messageHandle;
            //}

            //if (abiturient.IsOpfr == null)
            //{
            //    if (message.ToLower() == "да")
            //        abiturient.IsOpfr = true;
            //    if (message.ToLower() == "нет")
            //        abiturient.IsOpfr = false;

            //    if (message.ToLower() != "да" && message.ToLower() != "нет")
            //    {
            //        messageHandle.Message = answers.BadYesNoInput;
            //        messageHandle.ReplyKeyboard = keyboard.YesNoKeyboard;

            //        return messageHandle;
            //    }

            //    db.UpdateAbiturient(abiturient);

            //    messageHandle.Message = questions.IsHostel;
            //    messageHandle.ReplyKeyboard = keyboard.YesNoKeyboard;

            //    return messageHandle;
            //}

            //if (abiturient.IsNeedHostel == null)
            //{
            //    if (message.ToLower() == "да")
            //        abiturient.IsNeedHostel = true;
            //    if (message.ToLower() == "нет")
            //        abiturient.IsNeedHostel = false;

            //    if (message.ToLower() != "да" && message.ToLower() != "нет")
            //    {
            //        messageHandle.Message = answers.BadYesNoInput;
            //        messageHandle.ReplyKeyboard = keyboard.YesNoKeyboard;

            //        return messageHandle;
            //    }

            //    db.UpdateAbiturient(abiturient);

            //    messageHandle.Message = questions.InvalidGroup;
            //    messageHandle.ReplyKeyboard = keyboard.InvalidGroupKeyboard;

            //    return messageHandle;
            //}

            messageHandle.Message = answers.EndMessage;
            messageHandle.ReplyKeyboard = null;
            return messageHandle;
        }

        private InlineKeyboardButton ChangeButtonState(InlineKeyboardButton button)
        {
            string text = button.Text;

            // Changing text emoji to checkmark or cross
            if (text[text.Length - 1] == '✅')
            {
                text = text.Substring(0, text.Length - 1) + "❌";
            }
            else
            {
                text = text.Substring(0, text.Length - 1) + "✅";
            }

            // Changing callback to true or false
            string callback = button.CallbackData;

            int trueIndex = callback.IndexOf("True");

            if (trueIndex != -1)
            {
                callback = callback.Replace("False", "True");
            }
            else
            {
                callback = callback.Replace("True", "False");
            }

            InlineKeyboardButton newButton = new InlineKeyboardButton(text);
            newButton.CallbackData = callback;

            return newButton;
        }
    }
}
