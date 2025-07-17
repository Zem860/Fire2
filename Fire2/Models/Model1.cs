using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using Fire2.Areas.Front.Models;

namespace Fire2.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }
        public DbSet<Areas.Front.Models.AboutMenu> AboutMenus { get; set; }
        public DbSet<Areas.Front.Models.Experts> Experts { get; set; }

        public DbSet<Areas.Front.Models.NavbarItems> NavbarItems { get; set; }
        public DbSet<Areas.Front.Models.Members> Members { get; set; }
        public DbSet<Areas.Front.Models.ServiceHistory> ServiceHistories { get; set; }
        public DbSet<Areas.Front.Models.News> News { get; set; }
        public DbSet<Areas.Front.Models.AboutPageContent> AboutPageContents { get; set; }
        public DbSet<Areas.Front.Models.Posts> Posts { get; set; }
        public DbSet<Areas.Front.Models.Comments> Comments { get; set; }

        public DbSet<Areas.Dashboard.Models.Admins> Admins { get; set; }
        public DbSet<Areas.Dashboard.Models.Permission> Permissions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comments>()
                .HasRequired(c => c.Member)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MemberId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comments>()
                .HasRequired(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .WillCascadeOnDelete(true);
        }
    }
}
