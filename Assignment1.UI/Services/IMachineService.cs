using Assignment1.UI.Models;

namespace Assignment1.UI.Services
{
    public interface IMachineService
    {
        Task<List<string>> GetAllMachinesAsync();
        Task<List<string>> GetMachinesWithAssetAsync(Asset asset);
        Task<List<string>> GetMachinesWtihLatestAssetsAsync();
        Task<List<Asset>> GetAssetsForMachineTypeAsync(string type);
    }
}
