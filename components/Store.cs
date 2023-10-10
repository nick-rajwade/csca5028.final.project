using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace csca5028.final.project.components
{
    public class Store
    {
        //create a Store class that has a Name, Storelocation, GUID ID and a health check URL
        public String Name { get; set; }
        public StoreLocation StoreLocation { get; set; }
        public Guid ID { get; set; }
        public String HealthCheckURL { get; set; }

        public int HealthCheckInterval { get; set; }

        //create a constructor that takes all the fields as parameters
        public Store(String name, StoreLocation storeLocation, Guid id, String healthCheckURL) : this()
        {
            this.Name = name;
            this.StoreLocation = storeLocation;
            this.ID = id;
            this.HealthCheckURL = healthCheckURL;
        }

        public Store() 
        {
            //_inventory = _initialiseInventory(); DEPRECATED
        }
    }


    


}
