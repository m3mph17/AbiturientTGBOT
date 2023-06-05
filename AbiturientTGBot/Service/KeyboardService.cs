using AbiturientTGBot.Bot_QA;
using AbiturientTGBot.Models;
using Newtonsoft.Json;
using Telegram.Bot.Types.ReplyMarkups;
using InlineKeyboardMarkup = Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup;

namespace AbiturientTGBot.Service
{
    public class KeyboardService
    {
        // Data base code
        DBService db = new DBService();

        // Reply keyboards
        public ReplyKeyboardMarkup SpecialityKeyboard { get; private set; }
        public ReplyKeyboardMarkup ClassKeyboard { get; private set; }
        public ReplyKeyboardMarkup BaseSpecKeyboard { get; private set; }
        public ReplyKeyboardMarkup MidSpecKeyboard { get; private set; }
        public ReplyKeyboardMarkup TestKeyboard { get; private set; }
        public ReplyKeyboardMarkup IsMaleKeyboard { get; private set; }
        public ReplyKeyboardMarkup YesNoKeyboard { get; private set; }
        public ReplyKeyboardMarkup InvalidGroupKeyboard { get; private set; }

        // Next button
        private KeyboardButton[] NextButton = new KeyboardButton[] { "Далее" };

        // Inline keyboards
        public InlineKeyboardMarkup SocialInlineKeyboard { get; private set; }
        public InlineKeyboardMarkup ProfTestInlineKeyboard { get; private set; }

        // Inline buttons
        public InlineKeyboardButton[][] socialButtons { get; private set; }


        public KeyboardService()
        {
            SpecialityKeyboard = CreateSpecKeyboard();
            ClassKeyboard = CreateClassKeyboard();
            BaseSpecKeyboard = CreateBaseSpecKeyboard();
            MidSpecKeyboard = CreateMidSpecKeyboard();
            TestKeyboard = CreateTestKeyboard();
            IsMaleKeyboard = CreateIsMaleKeyboard();
            YesNoKeyboard = CreateYesNoKeyboard();
            InvalidGroupKeyboard = CreateInvalidGroupKeyboard();
            SocialInlineKeyboard = CreateSocialInlineKeyboard();
            socialButtons = CreateSocialButtons();
            ProfTestInlineKeyboard = CreateProfInlineKeyboard();
        }

        public InlineKeyboardMarkup CreateProfInlineKeyboard()
        {
            InlineKeyboardButton[] firstProfLink = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("Тест #1")
            };

            firstProfLink[0].Url = "https://testometrika.com/business/test-to-determine-career/";

            InlineKeyboardButton[] secondProfLink = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("Тест #2")
            };

            secondProfLink[0].Url = "https://www.profguide.io/test/category/proforient/";

            InlineKeyboardButton[] thirdProfLink = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("Тест #3")
            };

            thirdProfLink[0].Url = "https://spo.mosmetod.ru/test/1";

            InlineKeyboardButton[][] buttons = new InlineKeyboardButton[][]
            { firstProfLink, secondProfLink, thirdProfLink };

            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(buttons);

            return keyboard;
        }

        public InlineKeyboardMarkup CreateInlineKeyboard(InlineKeyboardButton[] buttons)
        {
            InlineKeyboardButton[] row1 = new InlineKeyboardButton[] { buttons[0], buttons[1] };
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(row1);

            return keyboard;
        }

        private InlineKeyboardButton[][] CreateSocialButtons()
        {
            InlineKeyboardButton[] isManyChildren = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("Многодетная ❌")
            };
            isManyChildren[0].CallbackData = "manyChildrenTrue";

            InlineKeyboardButton[] isOrphan = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("Сирота ❌")
            };
            isOrphan[0].CallbackData = "orphanTrue";

            InlineKeyboardButton[] isChernobyl = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("Чернобыль ❌")
            };
            isChernobyl[0].CallbackData = "chernobylTrue";

            InlineKeyboardButton[] isHostel = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("Хостел ❌")
            };
            isHostel[0].CallbackData = "hostelTrue";

            InlineKeyboardButton[] isOpfr = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("ОПФР ❌")
            };
            isOpfr[0].CallbackData = "opfrTrue";

            InlineKeyboardButton[] submit = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("Подтвердить информацию")
            };
            submit[0].CallbackData = "submitData";

            InlineKeyboardButton[][] keyboardButtons = new InlineKeyboardButton[][]
            { isManyChildren, isOrphan, isChernobyl, isHostel, isOpfr, submit };

            return keyboardButtons;
        }

        private InlineKeyboardMarkup CreateSocialInlineKeyboard()
        {
            // ❌ ✅

            InlineKeyboardButton[] isManyChildren = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("Многодетная ❌")
            };
            isManyChildren[0].CallbackData = "manyChildrenTrue";

            InlineKeyboardButton[] isOrphan = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("Сирота ❌")
            };
            isOrphan[0].CallbackData = "orphanTrue";

            InlineKeyboardButton[] isChernobyl = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("Чернобыль ❌")
            };
            isChernobyl[0].CallbackData = "chernobylTrue";

            InlineKeyboardButton[] isHostel = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("Хостел ❌")
            };
            isHostel[0].CallbackData = "hostelTrue";

            InlineKeyboardButton[] isOpfr = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("ОПФР ❌")
            };
            isOpfr[0].CallbackData = "opfrTrue";

            InlineKeyboardButton[] submit = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton("Подтвердить информацию")
            };
            submit[0].CallbackData = "submitData";

            InlineKeyboardButton[][] buttons = new InlineKeyboardButton[][]
            { isManyChildren, isOrphan, isChernobyl, isHostel, isOpfr, submit};

            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(buttons);

            return keyboard;
        }

        private ReplyKeyboardMarkup CreateInvalidGroupKeyboard()
        {
            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] {"Нет инвалидности"},
                new KeyboardButton[] {"1", "2", "3"},
            });

            keyboard.ResizeKeyboard = true;

            return keyboard;
        }

        private ReplyKeyboardMarkup CreateYesNoKeyboard()
        {
            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(new[]
{
                new KeyboardButton[] {"Да"},
                new KeyboardButton[] {"Нет"}
            });

            keyboard.ResizeKeyboard = true;

            return keyboard;
        }

        private ReplyKeyboardMarkup CreateIsMaleKeyboard()
        {
            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(new[]
{
                new KeyboardButton[] {"Мужской"},
                new KeyboardButton[] {"Женский"}
            });

            keyboard.ResizeKeyboard = true;

            return keyboard;
        }

        private ReplyKeyboardMarkup CreateTestKeyboard()
        {
            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] {"Тест проф. ориентации"},
                new KeyboardButton[] {"Заполнить заявку"}
            });

            keyboard.ResizeKeyboard = true;

            return keyboard;
        }

        private ReplyKeyboardMarkup CreateMidSpecKeyboard()
        {
            // разделение на 2 строки половина кнопок в первую, половина во вторую
            KeyboardButton[] firstRow;
            KeyboardButton[] secondRow;

            Specialization[] specializations = db.GetMidSpecializations();
            int firstLength = specializations.Count() / 2;
            int secondLength = specializations.Count() - firstLength;
            int specIndex = 0;

            firstRow = new KeyboardButton[firstLength];
            for (int i = 0; i < firstRow.Length; i++)
            {
                firstRow[i] = new KeyboardButton(specializations[i].Qualification);
                specIndex = i;
            }

            specIndex++;
            secondRow = new KeyboardButton[secondLength];
            for (int i = 0; i < secondRow.Length; i++)
            {
                secondRow[i] = new KeyboardButton(specializations[specIndex].Qualification);
                specIndex++;
            }

            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(new[]
            {
                firstRow,
                secondRow,
                NextButton
            });

            keyboard.ResizeKeyboard = true;
            keyboard.InputFieldPlaceholder = "Для просмотра доп. информации нажми одну из кнопок";

            return keyboard;
        }

        private ReplyKeyboardMarkup CreateBaseSpecKeyboard()
        {
            // разделение на 2 строки половина кнопок в первую, половина во вторую
            KeyboardButton[] firstRow;
            KeyboardButton[] secondRow;

            Specialization[] specializations = db.GetBaseSpecializations();
            int firstLength = specializations.Count() / 2;
            int secondLength = specializations.Count() - firstLength;
            int specIndex = 0;

            firstRow = new KeyboardButton[firstLength];
            for (int i = 0; i < firstRow.Length; i++)
            {
                firstRow[i] = new KeyboardButton(specializations[i].Qualification);
                specIndex = i;
            }

            specIndex++;
            secondRow = new KeyboardButton[secondLength];
            for (int i = 0; i < secondRow.Length; i++)
            {
                secondRow[i] = new KeyboardButton(specializations[specIndex].Qualification);
                specIndex++;
            }


            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(new[]
            {
                firstRow,
                secondRow,
                NextButton
            });

            keyboard.ResizeKeyboard = true;
            keyboard.InputFieldPlaceholder = "Для просмотра доп. информации нажми одну из кнопок";

            return keyboard;
        }

        private ReplyKeyboardMarkup CreateSpecKeyboard()
        {
            KeyboardButton[] firstRow;
            KeyboardButton[] secondRow;

            Specialization[] specializations = new Specialization[db.GetSpecializationsCount()];

            int firstLength = specializations.Count() / 2;
            int secondLength = specializations.Count() - firstLength;
            int specIndex = 0;

            Specialization[] baseSpecs = db.GetBaseSpecializations();
            Specialization[] midSpecs = db.GetMidSpecializations();

            firstRow = new KeyboardButton[baseSpecs.Length];
            secondRow = new KeyboardButton[midSpecs.Length];


            int startIndex = 0;
            for (int i = 0; i < baseSpecs.Length; i++)
            {
                specializations[i] = baseSpecs[i];
                startIndex++;
            }

            for (int i = 0; i < midSpecs.Length; i++)
            {
                specializations[startIndex] = midSpecs[i];
                startIndex++;
            }

            firstRow = new KeyboardButton[firstLength];
            for (int i = 0; i < firstRow.Length; i++)
            {
                firstRow[i] = new KeyboardButton(specializations[i].Qualification + " на базе " + specializations[i].ClassRequired + " классов");
                specIndex = i;
            }

            specIndex++;
            secondRow = new KeyboardButton[secondLength];
            for (int i = 0; i < secondRow.Length; i++)
            {
                secondRow[i] = new KeyboardButton(specializations[specIndex].Qualification + " на базе " + specializations[specIndex].ClassRequired + " классов");
                specIndex++;
            }

            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(new[]
            {
                firstRow,
                secondRow,
                NextButton
            });
            keyboard.ResizeKeyboard = true;

            keyboard.InputFieldPlaceholder = "Для просмотра доп. информации нажми одну из кнопок";

            return keyboard;
        }

        private ReplyKeyboardMarkup CreateClassKeyboard()
        {
            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] {"После 9-ого", "После 11-ого"},
                new KeyboardButton[] {"Показать все специальности"}
            });
            keyboard.ResizeKeyboard = true;

            return keyboard;
        }

        public void Save()
        {
            Specialization baseProgrammer = new Specialization
            {
                Name = "5-04-0612-02 Разработка и сопровождение программного обеспечения информационных систем",
                PrevName = "2-40 01 01 Программное обеспечение информационных технологий",
                Qualification = "Техник-программист",
                ClassRequired = 9,
                StudyTime = "3 года 10 месяцев",
                PrevPassScore = 8.8,
                StudentsAmount = 60,
                AddInfo = "",
                IsFree = false,
                FreeStudentsAmount = 0
            };

            Specialization midProgrammer = new Specialization
            {
                Name = "5-04-0612-02 Разработка и сопровождение программного обеспечения информационных систем",
                PrevName = "2-40 01 01 Программное обеспечение информационных технологий",
                Qualification = "Техник-программист",
                ClassRequired = 11,
                StudyTime = "2 года 10 месяцев",
                PrevPassScore = 0,
                FreeStudentsAmount = 8,
                AddInfo = "Абитуриентам с особенностями психофизического развития, зачисленным в колледж, предоставляется общежитие комитетом по образованию Мингорисполкома.",
                IsFree = true,
                StudentsAmount = 0
            };

            Specialization electronic = new Specialization
            {
                Name = "5-04-0713-08 Техническая эксплуатация технологического оборудования и средств робототехники в автоматизированном производстве",
                PrevName = "2-36 01 56 Мехатроника \n2 - 53 01 06 Промышленные роботы и робототехнические комплексы",
                Qualification = "Техник- электроник",
                ClassRequired = 9,
                StudyTime = "3 года 7 месяцев",
                PrevPassScore = 7.8,
                FreeStudentsAmount = 30,
                AddInfo = "1.По специальностям «Техническая эксплуатация технологического оборудования и средств робототехники в автоматизированном производстве» и " +
                "«Производство изделий микро- и наноэлектроники» базовой организацией ОАО «ИНТЕГРАЛ» - " +
                "управляющая компания холдинга «ИНТЕГРАЛ» (далее «ИНТЕГРАЛ») абитуриентам, успешно прошедшим испытания и зачисленным в колледж, будет предоставлено общежитие. " +
                "\n2.Предприятие также обеспечивает обучающимся прохождение оплачиваемых производственных практик.",
                IsFree = true,
                StudentsAmount = 0
            };

            Specialization baseTexnolog = new Specialization
            {
                Name = "5-04-0713-09 Производство изделий микро- и наноэлектроники",
                PrevName = "2-41 01 02 Микро- и наноэлектронные технологии и системы",
                Qualification = "Техник-технолог",
                ClassRequired = 9,
                StudyTime = "3 года 10 месяцев",
                PrevPassScore = 7.9,
                StudentsAmount = 50,
                AddInfo = electronic.AddInfo,
                IsFree = true,
                FreeStudentsAmount = 60
            };

            Specialization midTexnolog = new Specialization
            {
                Name = "5-04-0713-09 Производство изделий микро- и наноэлектроники",
                PrevName = "2-41 01 02 Микро- и наноэлектронные технологии и системы",
                Qualification = "Техник-технолог",
                ClassRequired = 11,
                StudyTime = "2 года 10 месяцев",
                PrevPassScore = 0,
                FreeStudentsAmount = 8,
                AddInfo = "Абитуриентам с особенностями психофизического развития, зачисленным в колледж, предоставляется общежитие комитетом по образованию Мингорисполкома.",
                IsFree = true,
                StudentsAmount = 0
            };

            Specialization electric = new Specialization
            {
                Name = "5-04-0712-01 Монтаж и эксплуатация электрооборудования",
                PrevName = "2-36 03 31 Монтаж и эксплуатация электрооборудования (по направлениям)",
                Qualification = "Техник-электрик",
                ClassRequired = 9,
                StudyTime = "3 года 7 месяцев",
                PrevPassScore = 7.4,
                FreeStudentsAmount = 30,
                AddInfo = "",
                IsFree = true,
                StudentsAmount = 0
            };

            List<Specialization> specializations = new List<Specialization>();
            specializations.Add(baseProgrammer);
            specializations.Add(midProgrammer);
            specializations.Add(electronic);
            specializations.Add(baseTexnolog);
            specializations.Add(midTexnolog);
            specializations.Add(electric);

            //specialityKeyboard.ResizeKeyboard = true;

            foreach (Specialization specialization in specializations)
            {
                string json = JsonConvert.SerializeObject(specialization, Formatting.Indented);
                //File.Create(@$"Specializations\{specialization.Qualification}" + "_" + specialization.ClassRequired + ".json");
                //File.AppendAllText(@$"Specializations\{specialization.Qualification}" + "_" + specialization.ClassRequired + ".json", json);
            }

            //string json = JsonConvert.SerializeObject(this);

            //File.Create("Keyboards.json");
            //File.AppendAllText("Keyboards.json", json);
        }
        public void LoadKeyabords()
        {

        }
    }
}
