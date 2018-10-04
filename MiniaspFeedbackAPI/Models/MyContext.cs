using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniaspFeedbackAPI.Models
{
    public class MyContext: DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }

        public DbSet<Feedback> Feedback { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Utoken> Utoken { get; set; }
    }
}
