using Assignment1.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assignment1.API.Controllers
{
    /// <summary>
    /// Controller for managing machines and their assets.
    /// </summary>
    [ApiController]
    [Route("api/v1/machines/")]
    public class MachineController : ControllerBase
    {
        private readonly IMachineService _machineService;

        /// <summary>
        /// Constructor for MachineController.
        /// </summary>
        /// <param name="service">Service for handling machine-related operations.</param>
        public MachineController(IMachineService service)
        {
            _machineService = service;
        }

        /// <summary>
        /// Returns the list of assets for a given machine type.
        /// </summary>
        /// <param name="machineType">The type of the machine for which to retrieve assets.</param>
        /// <returns>The list of assets for the specified machine type.</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [HttpGet("{machineType}/assets")]
        public async Task<IActionResult> GetAssetNamesForMachine(string machineType)
        {
            var result = await _machineService.GetAssetsForMachineAsync(machineType);
            return StatusCode(result.Status, result);
        }

        /// <summary>
        /// Returns the list of machines that are using the latest series of all their assets.
        /// </summary>
        /// <returns>The list of machine types that are using the latest series of all their assets.</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [HttpGet("filter/machine-using-latest-assets")]
        public async Task<IActionResult> GetFilteredMachineTypesAsync()
        {
            var result = await _machineService.GetMachinesUsingLatestSeriesAsync();
            return StatusCode(result.Status, result);
        }

        /// <summary>
        /// Returns the list of machines that are using a specific asset.
        /// </summary>
        /// <param name="asset">The asset to filter machines by.</param>
        /// <returns>The list of machine types that are using the specified asset.</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [HttpGet("filter/by-asset")]
        public async Task<IActionResult> GetFilteredMachiesByAsset([FromQuery] Asset asset)
        {
            var result = await _machineService.GetMachineTypesForAssetAsync(asset);
            return StatusCode(result.Status, result);
        }

        /// <summary>
        /// Returns the list of machine types.
        /// </summary>
        /// <returns>The list of machine types.</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<IActionResult> GetAllMachinesAsync()
        {
            var result = await _machineService.GetAllMachinesAsync();
            return StatusCode(result.Status, result);
        }

        /*[HttpPut("{machineType}/updateAsset")]

        ///<summary>
        ///Add or update asset in machine
        /// </summary>
        /// <param name="machineType">Machine tobe updated.</param>
        /// <param name="Asset">The asset record</param>
        ///<returns>Replaced machine object if successfully updated otherwise null</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddOrUpdateRecord([FromRoute] string machineType, [FromBody] Asset asset)
        {
            var result = await _machineService.AddOrUpdateAssetToMachineAsync(machineType, asset);
            return StatusCode(result.Status, result);
        }*/
    }
}
