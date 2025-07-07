using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

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
        public DbSet<Areas.Front.Models.ServiceHistory> serviceHistories { get; set; }
        public DbSet<Areas.Front.Models.News> News { get; set; }
        public DbSet<Areas.Front.Models.AboutPageContent> AboutPageContents { get; set; }

        public DbSet<Areas.Dashboard.Models.Admins> Admins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
