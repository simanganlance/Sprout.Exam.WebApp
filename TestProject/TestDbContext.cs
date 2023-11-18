using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.DataAccess.Repository.Models;

namespace TestProject
{
    

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }
        // Other DbSets or configurations as needed
    }
}
