using Microsoft.EntityFrameworkCore;

namespace Wpm.Clinic.Data_Access
{
    public class ClinicDbContext : DbContext 
    {
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
    : base(options) { }
        public DbSet<Consultation> Consultations { get; set; }
    }

    public record Consultation(Guid Id, 
        int PatientId,
        string PatientName, 
        DateTime StartTime);  

    public static class ClinicDbContextExtensions
    {
        public static void EnsureDbIsCreated(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<ClinicDbContext>();
            context.Database.EnsureCreated();
        }
    }   
}
