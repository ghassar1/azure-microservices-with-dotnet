using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using Wpm.Management.API.DataAccess;
namespace Wpm.Management.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController : ControllerBase
{
    private readonly ManagementDbContext _dbContext;
    private readonly ILogger<PetsController> _logger;

    public PetsController(ManagementDbContext dbContext, ILogger<PetsController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var all = await _dbContext.Pets.Include(p => p.breed).ToListAsync();
        return all != null ? Ok(all) : NotFound();
    }

    [HttpGet("{id}", Name = nameof(GetById))]
    public async Task<IActionResult> GetById(int id)
    {
        var pet = await _dbContext.Pets.Include(p => p.breed)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
        return pet != null ? Ok(pet) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewPet newPet)
    {
        try
        {
            var pet = newPet.toPet();
            await _dbContext.Pets.AddAsync(pet);
            await _dbContext.SaveChangesAsync();

            return CreatedAtRoute(nameof(GetById), new { id = pet.Id }, newPet);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.ToString());
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    public record class NewPet(string Name, int age, int breedId) { 
    public Pet toPet() {
            return new Pet() { Name = this.Name, age = this.age, breedId = this.breedId };
        }
    };
}
