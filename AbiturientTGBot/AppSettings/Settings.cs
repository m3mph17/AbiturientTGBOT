using AbiturientTGBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbiturientTGBot.AppSettings
{
    public class Settings
    {
        private string[] specPaths;
        public Specialization[] Specializations { get; private set; }
        public Settings()
        {
            GetSpecializationsPath();
        }

        public void SaveSpecializations()
        {
            ApplicationContext db = new ApplicationContext();
            
            for (int i = 0; i < Specializations.Length; i++)
            {
                db.Specializations.Add(Specializations[i]);
            }

            db.SaveChanges();
        }
        
        private void GetSpecializationsPath()
        {
            string path = @"Specializations"; 
            specPaths = Directory.GetFiles(path);
            Specializations = new Specialization[specPaths.Length];
        }

        public void LoadSpecializations()
        {
            for (int i = 0; i < specPaths.Length; i++)
            {
                Specializations[i] = new Specialization();
                string json = File.ReadAllText(specPaths[i]);
                Specializations[i] = JsonConvert.DeserializeObject<Specialization>(json);
            }
        }
    }
}
