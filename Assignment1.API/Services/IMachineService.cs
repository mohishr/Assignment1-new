namespace Assignment1.API.Services
{
    public interface IMachineService
    {
        Task<ServiceResponse<Machine>> AddOrUpdateAssetToMachineAsync(string machineId, Asset assetToAdd);
        Task<(bool Success, Machine RemovedMachine)> RemoveAssetFromMachineAsync(string machineId, string assetNameToRemove);
        Task<(bool Success, string DeletedMachineId)> DeleteMachineAsync(string machineId);
        Task<ServiceResponse<IEnumerable<Asset>>> GetAssetsForMachineAsync(string machineType);
        Task<ServiceResponse<IEnumerable<string>>> GetMachineTypesForAssetAsync(Asset asset);
        Task<ServiceResponse<IEnumerable<string>>> GetMachinesUsingLatestSeriesAsync();
    }
}
