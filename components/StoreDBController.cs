using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace csca5028.final.project.components
{
    public class StoreDBController
    {
        public async Task<IEnumerable<Store>> GetStoresAsync(string dbName, string connectionString)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                //fetch all fields from store and store location tables
                var sql = $"select * from [{dbName}].[dbo].Stores s inner join [{dbName}].[dbo].StoreLocation sl on s.store_location_id = sl.Id;";
                using(SqlCommand  command = connection.CreateCommand()) 
                { 
                    command.CommandText = sql;
                    using(SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        var stores = new List<Store>();
                        while (reader.Read())
                        {
                            var store = new Store();
                            store.ID = Guid.Parse(reader["Id"].ToString());
                            store.Name = reader["store_name"].ToString();
                            store.HealthCheckURL = reader["healthcheckurl"].ToString();
                            store.HealthCheckInterval = int.Parse(reader["healthcheckinterval"].ToString());
                            store.StoreLocation = new StoreLocation();
                            store.StoreLocation.Id = int.Parse(reader["store_location_id"].ToString());
                            store.StoreLocation.Address = reader["store_address"].ToString();
                            store.StoreLocation.City = reader["store_city"].ToString();
                            store.StoreLocation.State = reader["store_state"].ToString();
                            store.StoreLocation.Zip = reader["store_zip"].ToString();
                            store.StoreLocation.Country = reader["store_country"].ToString();
                            store.StoreLocation.Lat = decimal.Parse(reader["lat"].ToString());
                            store.StoreLocation.Long = decimal.Parse(reader["long"].ToString());
                            stores.Add(store);
                        }
                        return stores;
                    }
                }
            }
        }

		public async Task<DataTable> GetStoresDataTableAsync(string dbName, string connectionString)
		{
			DataSet dataSet = new DataSet();
            
            using (var connection = new SqlConnection(connectionString))
			{
				await connection.OpenAsync();
				//fetch all fields from store and store location tables
				var sql = $"select * from [{dbName}].[dbo].Stores s inner join [{dbName}].[dbo].StoreLocation sl on s.store_location_id = sl.Id;";
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = sql;
					using(SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
						adapter.Fill(dataSet,"Stores");
					}
				}
			}
            DataTable storeTable = dataSet.Tables["Stores"].Copy();
            return storeTable;
		}

		public async Task<IEnumerable<POSTerminal>> GetTerminalsAsync(Guid storeID, string dbName, string connectionString)
        {
            using(var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var sql = $"select * from [{dbName}].[dbo].pos_terminals where store_id = '{storeID.ToString()}';";
                using(SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    using(SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        var terminals = new List<POSTerminal>();
                        while(reader.Read())
                        {
                            var terminal = new POSTerminal();
                            terminal.posID = Guid.Parse(reader["Id"].ToString());
                            terminal.checkoutTime = int.Parse(reader["pos_checkout_time"].ToString());
                            terminal.storeID = Guid.Parse(reader["store_id"].ToString());
                            terminals.Add(terminal);
                        }
                        return terminals;
                    }
                }
            }
        }

        public async Task<Dictionary<string, Tuple<Store,List<POSTerminal>>>> GetStoresAndTerminalsAsync(string dbName, string connectionString)
        {
            var stores = (List<Store>)await GetStoresAsync(dbName,connectionString);
            var storesAndTerminals = new Dictionary<string, Tuple<Store, List<POSTerminal>>>();
            foreach (var store in stores)
            {
                var terminals = (List<POSTerminal>)await GetTerminalsAsync(store.ID, dbName, connectionString);
                storesAndTerminals.Add(store.Name, new Tuple<Store, List<POSTerminal>>(store, terminals));
            }
            return storesAndTerminals;
        }

        //get storeId by store_name
        public async Task<Guid> GetStoreIdAsync(string storeName, string dbName, string connectionString)
        {
            using(var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var sql = $"select Id from [{dbName}].[dbo].Stores where store_name = '{storeName}';";
                using(SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    using(SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if(reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                return Guid.Parse(reader["Id"].ToString());
                            }
                        }
                    }
                }
            }
            return Guid.Empty;
        }
    }
}
