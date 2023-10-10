namespace csca5028.final.project.components
{
    public class StoreLocation
    {
        //StoreLocation has properties that describe a store location (address, city, state, zip, country, lat, long)
        public String Address { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String Zip { get; set; }
        public String Country { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public int Id { get; set; }

        //create a constructor that takes all the fields as parameters
        public StoreLocation(String address, String city, String state, String zip, String country, decimal lat, decimal lon)
        {
            this.Address = address;
            this.City = city;
            this.State = state;
            this.Zip = zip;
            this.Country = country;
            this.Lat = lat;
            this.Long = lon;
        }

        public StoreLocation()
        {
        }

    }
}