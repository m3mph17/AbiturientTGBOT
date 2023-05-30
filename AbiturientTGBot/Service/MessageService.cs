using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using AbiturientTGBot.Models;

namespace AbiturientTGBot.Service
{
    public class MessageService
    {
        public string CreateInfoMessage(Specialization specialization)
        {
            string msg =
                $"Название специальности:\n{specialization.Name}\n" +
                $"Прошлое название:\n{specialization.PrevName}\n\n" +
                $"Квалификация: {specialization.Qualification}\n\n" +
                $"Срок обучения: {specialization.StudyTime}\n\n" +
                //specialization.PrevPassScore == 0 ? ("") : ("")
                $"Проходной балл в прошлом году: {specialization.PrevPassScore}\n\n" +
                $"Планируемый набор (платно): {specialization.StudentsAmount}\n" +
                $"Планируемый набор (бюджет): {specialization.FreeStudentsAmount}";


            if (String.IsNullOrEmpty(specialization.AddInfo) == false)
                msg += $"\n\nДоп. информация:\n {specialization.AddInfo}";

            return msg;
        }
    }
}
