namespace Wpm.Clinic.ExternalServices
{
    public class ManagementService
    {
        HttpClient _httpClient;

        public ManagementService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }   
        public async Task<PetInfo?> GetPetInfo(int petId)
        {
            var petInfo = await _httpClient.GetFromJsonAsync<PetInfo>($"api/pets/{petId}"); 
            return petInfo;
        }
    }

    public record class PetInfo(int Id, string Name, int age, int BreedId);
}
