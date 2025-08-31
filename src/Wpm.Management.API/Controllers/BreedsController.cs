using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using Wpm.Management.API.DataAccess;

namespace Wpm.Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreedsController : ControllerBase
    {
        private readonly ManagementDbContext _dbContext;
        private readonly ILogger<BreedsController> _logger;

        public BreedsController(ManagementDbContext dbContext, ILogger<BreedsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var all = await _dbContext.Breeds.ToListAsync();
            return all != null ? Ok(all) : NotFound();
        }

        [HttpGet("{id}", Name = nameof(GetBreedById))]
        public async Task<IActionResult> GetBreedById(int id)
        {
            var breed = await _dbContext.Breeds.FindAsync(id);
            return breed != null ? Ok(breed) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewBreed newBreed)
        {
            try
            {
                var breed = newBreed.ToBreed();
                await _dbContext.Breeds.AddAsync(breed);
                await _dbContext.SaveChangesAsync();

                return CreatedAtRoute(nameof(GetBreedById), new { id = breed.Id }, newBreed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
    public record NewBreed(string Name)
    {
        public Breed ToBreed()
        {
            return new Breed(0, Name);
        }
    }
}
