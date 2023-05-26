using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbiturientTGBot.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public long UserId { get; set; }
        public string State { get; set; }
        public string Username { get; set; }

    }
}
