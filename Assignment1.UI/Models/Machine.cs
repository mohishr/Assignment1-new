
namespace Assignment1.UI.Models
{

    public class Machine
    {
        public string Type { get; }
        public List<Asset> Assets { get; set; }
        public bool ShowDetails { get; set; } = false;

        public Machine()
        {
            
        }

        public Machine(string type)
        {
            Type=type;
        }
        public int GetAssetCount()
        {
            return Assets.Count;
        }
    }
}
