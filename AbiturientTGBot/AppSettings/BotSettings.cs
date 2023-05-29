using AbiturientTGBot.Bot_QA;
using AbiturientTGBot.Service;
using Newtonsoft.Json;

namespace AbiturientTGBot.AppSettings
{
    public class BotSettings
    {
        public string CfgPath { get; set; }
        public string AnswersPath { get; set; }
        public string AppQuestionsPath { get; set; }
        public KeyboardService KeyboardService { get; set; }
        public DBService DBService { get; set; }

        public string? Token { get; private set; }
        public Answers Answers { get; private set; }
        public ApplicationQuestions ApplicQuestions { get; private set; }

        private TokenSettings tokenSet;
        private AnswerSettings answerSet;
        private QuestionSettings questionSet;


        public BotSettings(string cfgPath, string answersPath, string appQuestionsPath, KeyboardService keyboard, DBService dbService)
        {
            CfgPath = cfgPath;
            AnswersPath = answersPath;
            AppQuestionsPath = appQuestionsPath;

            this.KeyboardService = keyboard;
            this.DBService = dbService;

            tokenSet = new TokenSettings(CfgPath);
            answerSet = new AnswerSettings(AnswersPath);
            questionSet = new QuestionSettings(appQuestionsPath);
        }

        public void LoadSettings()
        {
            tokenSet.LoadSettings();
            answerSet.LoadSettings();
            questionSet.LoadSettings();

            Token = tokenSet.Token;
            Answers = answerSet.Answers;
            ApplicQuestions = questionSet.ApplicQuestions;

            Console.WriteLine("\nНастройки бота успешно загружены\n\n");
        }
    }

    public class TokenSettings : ISettings
    {
        public string Path { get; set; }
        public string Token { get; private set; }

        public TokenSettings(string path)
        {
            Path = path;
        }

        public void LoadSettings()
        {
            if (File.Exists(Path) == false)
            {
                Console.WriteLine($"{Path} не найдено");
                throw new Exception($"{Path} file doesen't exists");
            }

            StreamReader reader = new StreamReader(Path);
            Token = reader.ReadLine();

            LoadSuccessPrint();
        }

        public void LoadSuccessPrint()
        {
            Console.WriteLine($"Токен бота из файла {Path} успешно загружен");
        }
    }

    public class AnswerSettings : ISettings
    {
        public string Path { get; set; }
        public Answers Answers { get; private set; }

        public AnswerSettings(string path)
        {
            Path = path;
        }

        public void LoadSettings()
        {
            if (File.Exists(Path) == false)
            {
                Console.WriteLine($"{Path} не найдено");
                throw new Exception($"{Path} file doesen't exists");
            }

            string json = File.ReadAllText(Path);
            Answers = JsonConvert.DeserializeObject<Answers>(json);

            LoadSuccessPrint();
        }

        public void LoadSuccessPrint()
        {
            Console.WriteLine($"Ответы бота из файла {Path} успешно загружены");
        }
    }

    public class QuestionSettings : ISettings
    {
        public string Path { get; set; }
        public ApplicationQuestions ApplicQuestions { get; private set; }

        public QuestionSettings(string path)
        {
            Path = path;
        }
        public void LoadSettings()
        {
            if (File.Exists(Path) == false)
            {
                Console.WriteLine($"{Path} не найдено");
                throw new Exception($"{Path} file doesen't exists");
            }

            string json = File.ReadAllText(Path);
            ApplicQuestions = JsonConvert.DeserializeObject<ApplicationQuestions>(json);

            LoadSuccessPrint();
        }

        public void LoadSuccessPrint()
        {
            Console.WriteLine($"Вопросы заявки из файла {Path} успешно загружены");
        }
    }
}
