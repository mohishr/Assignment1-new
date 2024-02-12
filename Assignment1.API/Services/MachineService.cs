using Assignment1.API.Repositories;
using Assignment1.Entities;

namespace Assignment1.API.Services
{
    public class MachineService : IMachineService
    {
        private static List<Asset> _latestAssetsList = new List<Asset>();
        private readonly IMachineRepo _machineRepository;

        public MachineService(IMachineRepo machineRepository)
        {
            _machineRepository = machineRepository;
        }

        public async Task<ServiceResponse<Machine>> AddOrUpdateAssetToMachineAsync(string machineType, Asset assetToAdd)
        {
            var serviceResponse = new ServiceResponse<Machine>();
            try
            {
                var machine = await _machineRepository.GetByTypeAsync(machineType);
                if (machine == null)
                {
                    serviceResponse.Data = null;
                    serviceResponse.Status = 404;
                    serviceResponse.Message = $"Machine {machineType} not found!!!";
                    return serviceResponse;
                }
                    

                machine.AddOrUpdateAsset(assetToAdd);

                var result = await _machineRepository.ReplaceOneAsync(machineType, machine);
                serviceResponse.Data = result.ReplacedObject;
                serviceResponse.Success = result.Success;
                if (serviceResponse.Success)
                {
                    serviceResponse.Status = 201;
                }
                else
                {
                    serviceResponse.Status = 500;
                }
                //UpdateLatestAssetsList(serviceResponse.Data.Assets);
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = 500;
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
                serviceResponse.Data = null;
            }

            return serviceResponse;
        }

        public async Task<(bool Success, Machine RemovedMachine)> RemoveAssetFromMachineAsync(string machineId, string assetNameToRemove)
        {
            try
            {
                var machine = await _machineRepository.GetByTypeAsync(machineId);
                if (machine == null)
                    return (false, null);

                var assetToRemove = machine.Assets.FirstOrDefault(asset => asset.Name == assetNameToRemove);
                if (assetToRemove == null)
                    return (false, null);

                machine.RemoveAsset(assetNameToRemove);

                var success = await _machineRepository.ReplaceOneAsync(machineId, machine);
                UpdateLatestAssetsList(success.ReplacedObject.Assets);
                return success;
            }
            catch (Exception ex)
            {
                return (false, null);
            }
        }

        public async Task<(bool Success, string DeletedMachineId)> DeleteMachineAsync(string machineId)
        {
            try
            {
                var success = await _machineRepository.DeleteByTypeAsync(machineId);
                return (success, success ? machineId : null);
            }
            catch (Exception ex)
            {
                return (false, null);
            }
        }

        public async Task<ServiceResponse<IEnumerable<Asset>>> GetAssetsForMachineAsync(string machineType)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<Asset>>();
            try
            {
                var machine = await _machineRepository.GetByTypeAsync(machineType);
                if (machine is null)
                {
                    serviceResponse.Success = true;
                    serviceResponse.Status = 404;
                    serviceResponse.Data = null;
                    serviceResponse.Message = $"Machine : {machineType} not found";
                }
                else
                {
                    serviceResponse.Data = machine.Assets;
                    serviceResponse.Status = 200;
                    serviceResponse.Success = true;
                    serviceResponse.Message = $"List of Assets in machine :{machine}";
                }

                return serviceResponse;

            }
            catch (Exception ex)
            {
                serviceResponse.Status = 500;
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;

            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<string>>> GetMachineTypesForAssetAsync(Asset asset)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<string>>();
            try
            {
                var machines = await _machineRepository.GetMachinesByAssetAsync(asset);
                serviceResponse.Data = machines.Select(machine => machine.Type);
                serviceResponse.Success = true;
                serviceResponse.Status = 200;
                if (serviceResponse.Data.Count() == 0)
                {
                    serviceResponse.Message = $"No machine uses asset: {asset.ToString()}";
                }
                else
                {
                    serviceResponse.Message = $"List of machines with asset: {asset.ToString()}";
                }

                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = 500;
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
                serviceResponse.Data = null;

            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<string>>> GetMachinesUsingLatestSeriesAsync()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<string>>();
            try
            {
                var machines = await _machineRepository.GetAllMachinesAsync();
                machines.AsQueryable().ToList().ForEach(m=>
                    UpdateLatestAssetsList(m.Assets));
                var t = _latestAssetsList;
                var result = machines.Where(m =>
                    m.Assets.All(a => _latestAssetsList.Any(latestAsset => a.Name==latestAsset.Name && a.SeriesNumber==latestAsset.SeriesNumber))).Select(m => m.Type);
                serviceResponse.Data = result;
                serviceResponse.Status = 200;
                serviceResponse.Success = true;
                if (result.Count() == 0)
                {
                    serviceResponse.Message = "No machine is up to date";
                }
                else
                {
                    serviceResponse.Message = "List of machines up to date";
                }

                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = 500;
                serviceResponse.Message = ex.Message;
                
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<string>>> GetAllMachinesAsync()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<string>>();
            try
            {
                var res = await _machineRepository.GetAllMachinesAsync();
                serviceResponse.Data = res.Select(m => m.Type);
                serviceResponse.Status = 200;
                serviceResponse.Success = true;
                serviceResponse.Message = "List of all available machines";
            }
            catch (Exception ex){
                serviceResponse.Status = 500;
                serviceResponse.Message = ex.Message;

            }

            return serviceResponse;

        }

        private void UpdateLatestAssetsList(List<Asset> assets)
        {
            foreach (Asset asset in assets)
            {
                if (_latestAssetsList.Count == 0)
                {
                    _latestAssetsList.Add(new(asset));
                    return;
                }

                var oldAsset = _latestAssetsList.FirstOrDefault(a =>
                    string.Equals(a.Name, asset.Name, StringComparison.OrdinalIgnoreCase));
                if (oldAsset != null)
                {
                    int currentVersion = int.Parse(asset.SeriesNumber.Substring(1));
                    int prevVersion = int.Parse(oldAsset.SeriesNumber.Substring(1));
                    oldAsset.SeriesNumber = currentVersion > prevVersion ? asset.SeriesNumber : oldAsset.SeriesNumber;
                }
                else
                {
                    _latestAssetsList.Add(new(asset));
                }
            }

        }
    }

}
