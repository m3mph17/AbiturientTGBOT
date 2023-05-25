using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AbiturientTGBot.Models
{
    public class Specialization
    {
        [Key]
        public int SpecId { get; set; }
        public string Name { get; set; }
        public string PrevName { get; set; }
        public string Qualification { get; set; }
        public string StudyTime { get; set; }
        public string AddInfo { get; set; }
        public double PrevPassScore { get; set; }
        public int StudentsAmount { get; set; }
        public int FreeStudentsAmount { get; set; }
        public int ClassRequired { get; set; }
        public bool IsFree { get; set; }
    }
}
