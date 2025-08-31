using Microsoft.EntityFrameworkCore;

namespace Wpm.Management.API.DataAccess
{
    public class ManagementDbContext : DbContext
    {
        public ManagementDbContext(DbContextOptions<ManagementDbContext> options)
       : base(options) { }


        public DbSet<Pet> Pets { get; set; }
        public DbSet<Breed> Breeds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Breed>().HasData(
                new Breed(1, "Labrador"),
                new Breed(2, "German Shepherd"),
                new Breed(3, "Golden Retriever")
            );
            modelBuilder.Entity<Pet>().HasData(
                new Pet { Id = 1, Name = "Buddy", age = 3, breedId = 1 },
                new Pet { Id = 2, Name = "Max", age = 5, breedId = 2 },
                new Pet { Id = 3, Name = "Bella", age = 2, breedId = 3 },
                new Pet { Id = 4, Name = "Snoopy", age = 50, breedId = 1 }
            );
        }
    }

    public static class ManagementDbContextExtensions
    {
        public static void EnsureDbIsCreated(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var content = scope.ServiceProvider.GetService<ManagementDbContext>();
            content.Database.EnsureCreated();
        }
    }

    public class Pet {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int age { get; set; }

        public int breedId { get; set; }
        public Breed breed { get; set; }

    }
    public record Breed(int Id, string name);
}
