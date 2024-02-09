

namespace Assignment1.API.Repositories
{
    public interface IMachineRepo
    {
            Task<(bool Success, Machine ReplacedObject)> InsertOneAsync(Machine machine);
            Task<Machine> GetByTypeAsync(string type);
            Task<(bool Success, Machine ReplacedObject)> ReplaceOneAsync(string id, Machine replacement);
            Task<bool> DeleteByTypeAsync(string type);
            Task<IEnumerable<Machine>> GetMachinesByAssetAsync(Asset assetName);
            Task<IEnumerable<Machine>> GetAllMachinesAsync();
    }
}
