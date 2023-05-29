using AbiturientTGBot.AppSettings;
using AbiturientTGBot.Bot_QA;
using AbiturientTGBot.Models;
using AbiturientTGBot.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbiturientTGBot.Handlers
{
    public class ApplicationHandler
    {
        User user;
        string message;
        BotService bot;

        // Services
        DBService db;
        KeyboardService keyboard;
        ApplicationQuestions questions;

        public ApplicationHandler(object sender, long userId, string message)
        {
            bot = (BotService)sender;
            db = bot.db;
            keyboard = bot.keyboard;
            user = bot.db.GetUser(userId);

            this.message = message;
            questions = bot.settings.ApplicQuestions;
        }

        public MessageHandle Handle()
        {
            Abiturient abiturient = db.GetAbiturient(user.Id);
            MessageHandle messageHandle = new MessageHandle { User = user};

            if (abiturient.IsFull == true)
            {
                messageHandle.Message = bot.settings.Answers.ApplicationFulled;
                messageHandle.ReplyKeyboard = null;

                return messageHandle;
            }

            if (abiturient == null)
            {
                abiturient = new Abiturient { UserId = user.Id };
                db.CreateNewAbiturient(abiturient);

                messageHandle.Message = questions.Fio;
                messageHandle.ReplyKeyboard = null;

                return messageHandle;
            }

            if (abiturient.Surname == null)
            {
                string[] fio = message.Split(' ');
                abiturient.Surname = fio[0];
                abiturient.Firstname = fio[1];
                abiturient.Patronymic = fio[2];

                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.IsMale;
                messageHandle.ReplyKeyboard = keyboard.IsMaleKeyboard;

                return messageHandle;
            }

            if (abiturient.IsMale == null)
            {
                if (message.ToLower() == "мужской")
                    abiturient.IsMale = true;

                if (message.ToLower() == "женский")
                    abiturient.IsMale = false;

                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.Address;
                messageHandle.ReplyKeyboard = null;

                return messageHandle;
            }

            if (abiturient.Address == null)
            {
                abiturient.Address = message;
                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.HomeNumber;
                messageHandle.ReplyKeyboard = null;

                return messageHandle;
            }

            if (abiturient.HomePhone == null)
            {
                abiturient.HomePhone = Convert.ToInt64(message);
                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.PhoneNumber;
                messageHandle.ReplyKeyboard = null;

                return messageHandle;
            }

            if (abiturient.MobilePhone == null)
            {
                abiturient.MobilePhone = Convert.ToInt64(message);
                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.Birthday;
                messageHandle.ReplyKeyboard = null;

                return messageHandle;
            }


            if (abiturient.BirthDate == null)
            {
                abiturient.BirthDate = message;
                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.IsOrphan;
                messageHandle.ReplyKeyboard = keyboard.YesNoKeyboard;

                return messageHandle;
            }

            if (abiturient.IsOrphan == null)
            {
                if (message.ToLower() == "да")
                    abiturient.IsOrphan = true;
                else
                    abiturient.IsOrphan = false;

                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.IsChernobyl;
                messageHandle.ReplyKeyboard = keyboard.YesNoKeyboard;

                return messageHandle;
            }

            if (abiturient.IsFromChernobyl == null)
            {
                if (message.ToLower() == "да")
                    abiturient.IsFromChernobyl = true;
                else
                    abiturient.IsFromChernobyl = false;

                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.IsManyChildren;
                messageHandle.ReplyKeyboard = keyboard.YesNoKeyboard;

                return messageHandle;
            }

            if (abiturient.IsManyChildren == null)
            {
                if (message.ToLower() == "да")
                    abiturient.IsManyChildren = true;
                else
                    abiturient.IsManyChildren = false;

                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.IsOpfr;
                messageHandle.ReplyKeyboard = keyboard.YesNoKeyboard;

                return messageHandle;
            }

            if (abiturient.IsOpfr == null)
            {
                if (message.ToLower() == "да")
                    abiturient.IsOpfr = true;
                else
                    abiturient.IsOpfr = false;

                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.IsHostel;
                messageHandle.ReplyKeyboard = keyboard.YesNoKeyboard;

                return messageHandle;
            }

            if (abiturient.IsNeedHostel == null)
            {
                if (message.ToLower() == "да")
                    abiturient.IsNeedHostel = true;
                else
                    abiturient.IsNeedHostel = false;
                    
                db.UpdateAbiturient(abiturient);

                messageHandle.Message = questions.InvalidGroup;
                messageHandle.ReplyKeyboard = keyboard.InvalidGroupKeyboard;

                return messageHandle;
            }

            if (abiturient.InvalidGroup == null)
            {
                if (message.ToLower() == "нет инвалидности")
                    abiturient.InvalidGroup = 0;
                else
                    abiturient.InvalidGroup = Convert.ToInt32(message);

                abiturient.IsFull = true;
                db.UpdateAbiturient(abiturient);

                messageHandle.Message = bot.settings.Answers.EndMessage;
                messageHandle.ReplyKeyboard = null;

                db.SetUserStateByTableId(user.Id, "newUser");

                return messageHandle;
            }

            messageHandle.Message = bot.settings.Answers.EndMessage;
            messageHandle.ReplyKeyboard = null;
            return messageHandle;
        }
    }
}
