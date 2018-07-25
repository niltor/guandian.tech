using System;
using System.Collections.Generic;
using System.Text;
using Functions.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Functions.Data
{
    public class NewsDbContext : DbContext
    {
        public DbSet<News> News { get; set; }

        readonly string _connectionString;
        public NewsDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
