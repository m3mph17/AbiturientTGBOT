using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbiturientTGBot.Models
{
    public class Application
    {
        public string SpecName { get; set; }
        public int ClassReq { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Address { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public double SchoolMark { get; set; }
        public bool IsMale { get; set; }
        public bool IsOrphan { get; set; }
        public bool IsManyChildren { get; set; }
        public bool IsOpfr { get; set; }
        public int InvalidGroup { get; set; }
        public bool IsFromChernobyl { get; set; }
        public bool IsNeedHostel { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string FillingDate { get; set; }
    }
}
