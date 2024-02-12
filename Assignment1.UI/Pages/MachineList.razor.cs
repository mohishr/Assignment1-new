using Microsoft.AspNetCore.Components;

namespace Assignment1.UI.Pages
{
    partial class MachineList:ComponentBase
    {
        
        [Parameter]
        public IEnumerable<Machine> Machines { get; set; }
        [Inject]
        private IMachineService _service { get; set; }
        private async Task GetAssetList(string type)
        {
            
            var response = await _service.GetAssetsForMachineTypeAsync(type);
            Machines.First(m => m.Type == type).Assets = response;
            StateHasChanged();
        }


    }
    
}
