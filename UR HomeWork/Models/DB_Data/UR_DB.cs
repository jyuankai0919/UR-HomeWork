using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace UR_HomeWork.Models.DB_Data
{
    public partial class UR_DB : DbContext
    {
        public UR_DB()
            : base("name=UR_DB")
        {
        }

        public virtual DbSet<User> User { get; set; }

        public DbSet<Tokens> Tokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tokens>()
        .Property(t => t.UserId)
        .HasMaxLength(50);
        }
    }
}
