using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_Resume.Models;

namespace Core_Resume.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
        {
        }

        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }
        public DbSet<Registration> Register { get; set; }
        public DbSet<TempEmail> TempEmail { get; set; }
        public DbSet<Persnola> Persnol { get; set; }
        
        public DbSet<photo> Images { get; set; }
        public DbSet<LanAndHob> LanAndHobs { get; set; }
        public DbSet<Core_Resume.Models.Educational> Educational { get; set; }
        public DbSet<Core_Resume.Models.WorkHistory> WorkHistory { get; set; }
        public DbSet<Core_Resume.Models.Summry_CarrerObjective> Summry_CarrerObjective { get; set; }

        public DbSet<Core_Resume.Models.Skills> Skills { get; set;  }

        public DbSet<Core_Resume.Models.ProjectDetails> ProjectDetails { get; set; }

    }
}
