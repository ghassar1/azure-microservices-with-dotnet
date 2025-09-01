using Wpm.Clinic.Controllers;
using Wpm.Clinic.Data_Access;
using Wpm.Clinic.ExternalServices;

namespace Wpm.Clinic.Application
{
    public class ClinicApplicationService
    {
        private ClinicDbContext ClinicDbContext; 
        private ManagementService ManagementService;

        public ClinicApplicationService(ClinicDbContext clinicDbContext,
            ManagementService managementService)
        {
            ClinicDbContext = clinicDbContext;
            ManagementService = managementService;
        }   

        public async Task<Consultation?> Handle(StartConsultationCommand command)
        {
            var petInfo = await ManagementService.GetPetInfo(command.PatientId);

            var newConsultation = new Consultation(
              Guid.NewGuid(),
              command.PatientId,
              petInfo.Name,
              DateTime.UtcNow
            );
            await ClinicDbContext.Consultations.AddAsync(newConsultation);
            await ClinicDbContext.SaveChangesAsync();
            return newConsultation; 
        }   
    }
}
