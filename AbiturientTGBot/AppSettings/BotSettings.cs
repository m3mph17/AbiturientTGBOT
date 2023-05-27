using Newtonsoft.Json;

namespace AbiturientTGBot.AppSettings
{
    public class BotSettings : ISettings
    {
        public string path = @"Settings\Bot.cfg";
        public string? Token { get; set; }

        public void SaveSpecializations()
        {
            //ApplicationContext db = new ApplicationContext();

            //for (int i = 0; i < Specializations.Length; i++)
            //{
            //    db.Specializations.Add(Specializations[i]);
            //}

            //db.SaveChanges();
        }

        public void LoadSettings()
        {
            if (File.Exists(path) == false)
            {
                Console.WriteLine("Bot.cfg не найден");
                throw new Exception("Bot.cfg file doesen't exists");
            }

            StreamReader reader = new StreamReader(path);
            Token = reader.ReadLine();

            //for (int i = 0; i < specPaths.Length; i++)
            //{
            //    Specializations[i] = new Specialization();
            //    string json = File.ReadAllText(specPaths[i]);
            //    Specializations[i] = JsonConvert.DeserializeObject<Specialization>(json);
            //}
        }
    }
}
