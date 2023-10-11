namespace csca5028BlazorApp2.Data
{
    public class MarkerData
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string name { get; set; } = string.Empty;
        public decimal salesPerf { get; set; } = 0.0M;
        public int txnsInInterval { get; set; } = 0;
        public int posCount { get; set; } = 0;
    }
}
