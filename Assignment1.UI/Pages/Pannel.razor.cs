using Microsoft.AspNetCore.Components;

namespace Assignment1.UI.Pages
{
    partial class Pannel:ComponentBase
    {
        public Asset? asset { get; set; } = new Asset();
        [Inject]
        private IMachineService _service { get; set; }
        private List<Machine> Machines { get; set; } = new List<Machine>();
        public bool assetFetchFlag => (asset.Name != null && asset.Name.Length !=0 && asset.SeriesNumber != null && asset.SeriesNumber.Length!=0)  ? false : true ;
        
        protected override async void OnInitialized()
        {
            var res = await _service.GetAllMachinesAsync();
            foreach (var re in res)
            {
                Machines.Add(new Machine(re));
            }
            StateHasChanged();
        }

        private async void GetMachineByAsset()
        {
            var res = await _service.GetMachinesWithAssetAsync(asset);
            foreach (var re in res)
            {
                Machines.Add(new Machine(re));
            }
            StateHasChanged();
        }

        private async void GetLatestMachines()
        {
            var res = await _service.GetMachinesWtihLatestAssetsAsync();
            foreach (var re in res)
            {
                Machines.Add(new Machine(re));
            }
            StateHasChanged();
        }

        private async void GetAllMachines()
        {
            var res = await _service.GetAllMachinesAsync();
            foreach (var re in res)
            {
                Machines.Add(new Machine(re));
            }
            StateHasChanged();
        }

    }
}
