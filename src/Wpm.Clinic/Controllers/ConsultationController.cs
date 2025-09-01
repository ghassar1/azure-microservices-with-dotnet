using Microsoft.AspNetCore.Mvc;
using Wpm.Clinic.Application;

namespace Wpm.Clinic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationController : ControllerBase
    {
        private ClinicApplicationService ClinicApplicationService;
        public ConsultationController(ClinicApplicationService clinicApplicationService)
        {
            ClinicApplicationService = clinicApplicationService;
        }   
        [HttpPost("/start")]
       public async Task<IActionResult> Start(StartConsultationCommand command)
       {
            var result = await ClinicApplicationService.Handle(command);
            if (result == null)
            {
                return BadRequest("Could not start consultation");
            }   
            return Ok(result);
        }
    }

    public record StartConsultationCommand(int PatientId);
}
