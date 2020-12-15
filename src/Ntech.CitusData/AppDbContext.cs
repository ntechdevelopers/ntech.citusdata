using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ntech.CitusData.Models;
using Npgsql.NameTranslation;

namespace Ntech.CitusData
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var mapper = new NpgsqlSnakeCaseNameTranslator();
            var types = modelBuilder.Model.GetEntityTypes().ToList();

            // Refer to tables in snake_case internally
            types.ForEach(e => e.SetTableName(mapper.TranslateMemberName(e.GetTableName())));
            
            // Refer to columns in snake_case internally
            types.SelectMany(e => e.GetProperties())
                .ToList()
                .ForEach(p => p.SetColumnName(mapper.TranslateMemberName(p.GetColumnName())));
        }
    }
}
