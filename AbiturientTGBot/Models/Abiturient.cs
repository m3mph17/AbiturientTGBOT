using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbiturientTGBot.Models
{
    public class Abiturient
    {
        public int AbiturientId { get; set; }
        public int UserId { get; set; }
        public int? SpecId { get; set; }
        public bool IsFull { get; set; }
        public string? Surname { get; set; }
        public string? Firstname { get; set; }
        public string? Patronymic { get; set; }
        public string? Address { get; set; }
        public string? SchoolName { get; set; }
        public string? SchoolAddress { get; set; }
        public double? SchoolMark { get; set; }
        public bool? IsMale { get; set; }
        public bool? IsOrphan { get; set; }
        public bool? IsManyChildren { get; set; }
        public bool? IsOpfr { get; set; }
        public int? InvalidGroup { get; set; }
        public bool? IsFromChernobyl { get; set; }
        public bool? IsNeedHostel { get; set; }
        public long? HomePhone { get; set; }
        public long? MobilePhone { get; set; }
        public string? BirthDate { get; set; }
        public DateTime? FillingDate { get; set; }
    }
}
