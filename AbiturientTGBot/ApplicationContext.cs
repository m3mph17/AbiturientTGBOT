using AbiturientTGBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbiturientTGBot
{
    public class ApplicationContext : DbContext
    {
        // Tables of data base
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Specialization> Specializations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=AbiturientTG;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
