using Microsoft.Identity.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csca5028.final.project.components
{
    public class SalesAnalyzer : ISalesAnalyzer, IDisposable
    {
        private string connectionString = string.Empty;
        private string dbName = "sales_db";
        //calculate the total sales performance for all stores

        public SalesAnalyzer(string dbName, string connnectionString)
        {
            this.dbName = dbName;
            this.connectionString = connnectionString;
        }

        public async Task<Dictionary<string, Tuple<int, decimal>>> LoadSalesPerformance()
        {
            SalesDBController salesDBController = new SalesDBController();

            var storeSalesPerf = await salesDBController.GetSalesPerformanceForTimeInterval(dbName, 5, connectionString); //look at last 5 minutes only
            return storeSalesPerf;
        }

        public async Task<decimal> GetTotalRevenueForTimeInterval()
        {
            decimal totalSalesPerformance = 0;
            var storeSalesPerf = await LoadSalesPerformance();

            for (int i = 0; i < storeSalesPerf.Count; i++)
            {
                totalSalesPerformance += storeSalesPerf.ElementAt(i).Value.Item2;
            }

            return totalSalesPerformance;
        }

        public async Task<int> GetTransactionsPerMinuteAcrossAllStores()
        {
            int transactionsPerMinute = 0;
            var storeSalesPerf = await LoadSalesPerformance();
            for (int i = 0; i < storeSalesPerf.Count; i++)
            {
                transactionsPerMinute += storeSalesPerf.ElementAt(i).Value.Item1;
            }

            return transactionsPerMinute;
        }

        public async Task<decimal> GetRevenueForStore(string storeName)
        {
            decimal revenueForStore = 0;
            var storeSalesPerf = await LoadSalesPerformance();
            if(storeSalesPerf.ContainsKey(storeName))
            {
                revenueForStore = storeSalesPerf[storeName].Item2;
            }
            return revenueForStore;
        }

        public async Task<int> GetTransactionsPerMinuteForStore(string storeName)
        {
            int transactionsPerMinuteForStore = 0;
            var storeSalesPerf = await LoadSalesPerformance();
            if (storeSalesPerf.ContainsKey(storeName))
                transactionsPerMinuteForStore = storeSalesPerf[storeName].Item1;
            return transactionsPerMinuteForStore;
        }

        public void Dispose()
        {

        }

        public async Task SetSalesPerformance(Dictionary<string, Tuple<int, decimal>> salesPerformance)
        {
            await Task.CompletedTask;
        }

        public async Task<Dictionary<string, Tuple<decimal, decimal>>> GetStoreLocations()
        {
            StoreDBController storeDBController = new StoreDBController();
            Dictionary<string,Tuple<Store,List<POSTerminal>>> storeAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName,connectionString);
            Dictionary<string, Tuple<decimal, decimal>> storeLocations = new();
            foreach (string storeName in storeAndTerminals.Keys)
            {
                storeLocations.Add(storeName, new Tuple<decimal, decimal>(
                    storeAndTerminals[storeName].Item1.StoreLocation.Lat,
                    storeAndTerminals[storeName].Item1.StoreLocation.Long));
            }
            return storeLocations;
        }

        public async Task SetStoreLocations(Dictionary<string, Tuple<decimal, decimal>> storeLocations)
        {
            await Task.CompletedTask;
        }

        public async Task<Dictionary<string, Tuple<Store, List<POSTerminal>>>> GetStoresAndTerminals()
        {
            StoreDBController storeDBController = new StoreDBController();
            
            Dictionary<string, Tuple<Store, List<POSTerminal>>> storesAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName, connectionString);
            
            return storesAndTerminals;
        }

        public async Task SetStoresAndTerminals(Dictionary<string, Tuple<Store, List<POSTerminal>>> storesAndTerminals)
        {
            await Task.CompletedTask;
        }

        public async Task<decimal> GetTotalSalesRevenue()
        {
            SalesDBController salesDBController = new SalesDBController();
            return await salesDBController.GetTotalSalesRevenue(dbName,connectionString);
        }

        public async Task SetTotalSalesRevenue(decimal totalSalesRevenue)
        {
            await Task.CompletedTask;
        }
    }
}
