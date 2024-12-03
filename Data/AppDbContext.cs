using DepartmentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DepartmentAPI.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasKey(d => d.DeptID);
        }
    }
}
