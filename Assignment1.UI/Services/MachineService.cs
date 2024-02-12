using Assignment1.UI.Models;
using System.Net.Http;

namespace Assignment1.UI.Services
{
    public class MachineService:IMachineService
    {
        private readonly HttpClient _client;
        public MachineService(HttpClient client)
        {
            _client = client;   
        }

        public async Task<List<string>> GetAllMachinesAsync()
        {
            var res = await _client.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/v1/machines");
            return res.Data;
        }


        public async Task<List<string>> GetMachinesWithAssetAsync(Asset asset)
        {
            var response = await _client.GetAsync($"api/v1/machines/filter/by-asset?Name={asset.Name}&SeriesNumber={asset.SeriesNumber}");
            //response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadFromJsonAsync<ServiceResponse<List<string>>>();
            return responseContent.Data;
        }

        public async Task<List<string>> GetMachinesWtihLatestAssetsAsync()
        {
            var response = await _client.GetAsync("/api/v1/machines/filter/machine-using-latest-assets");
            var res =  await response.Content.ReadFromJsonAsync<ServiceResponse<List<string>>>();
            return res.Data;
        }

        public async Task<List<Asset>> GetAssetsForMachineTypeAsync(string machineType)
        {
            var response = await _client.GetAsync($"api/v1/machines/{machineType}/assets");
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadFromJsonAsync<ServiceResponse<List<Asset>>>();
            return res.Data;
        }

    }
}
