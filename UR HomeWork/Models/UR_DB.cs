using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace UR_HomeWork.Models
{
    public partial class UR_DB : DbContext
    {
        public UR_DB()
            : base("name=UR_DB")
        {
        }

        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.Account)
                .IsUnicode(false);
        }
    }
}
