using RAPSimple.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;

namespace RAPSimple.DAL
{
    public class RAPInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<RAPDbContext>
    {
        protected override void Seed(RAPDbContext context)
        {
            var students = new List<Profile>
            {
            new Profile{FirstMidName="Carson",LastName="Alexander", Details="Test user"},
            new Profile{FirstMidName="Meredith",LastName="Alonso", Details="Test user"}
            };

            students.ForEach(s => context.Profiles.Add(s));
            context.SaveChanges();
        }
    }

    public class RAPDbContext : DbContext
    {
        public RAPDbContext()
            : base("DBContext")
        {
        }
        //        public DbSet<Profile> Persons { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
#if false
            modelBuilder.Entity<Event>()
      .HasRequired(a => a.Profile)
      .WillCascadeOnDelete(false);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
#endif

            modelBuilder.Entity<Profile>().ToTable("Profile");
            modelBuilder.Entity<Event>().ToTable("Events");
            modelBuilder.Entity<File>().ToTable("File");

        }

        public System.Data.Entity.DbSet<RAPSimple.Models.Group> Groups { get; set; }
    }
}