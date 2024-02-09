namespace Assignment1.Entities
{
    public class Asset
    {
        public string Name { get; set; }
        public string SeriesNumber { get; set; }

        public Asset() { }
        public Asset(string name, string seriesNumber)
        {
            Name = name;
            SeriesNumber = seriesNumber;
        }
        public Asset(Asset other)
        {
            Name = other.Name;
            SeriesNumber = other.SeriesNumber;
        }
        public bool valueEquals(Asset other)
        {
            if (other == null) return false;
            return other.Name.ToLower() == this.Name.ToLower() &&
                   other.SeriesNumber.ToLower() == this.SeriesNumber.ToLower();
        } 
        public override string ToString()
        {
            return $"{Name}, Series: {SeriesNumber}";
        }
    }

}
