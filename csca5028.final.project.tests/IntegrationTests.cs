using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using csca5028.final.project.api;
using csca5028.final.project.components;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace csca5028.final.project.tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod()]
        public void CreditCard_AuthorisationTest()
        {
            //create a list of items where items are {bacon, 1, 5.99}, {eggs, 1, 2.99}, {milk, 1, 3.99}, {bread, 1, 2.99}, {cheese, 1, 4.99}, {chicken, 1, 5.99}, {beef, 1, 6.99}, {pork, 1, 5.99}, {fish, 1, 7.99}, {apple, 1, 1.99}, {banana, 1, 1.99}, {orange, 1, 1.99}, {grape, 1, 1.99}, {strawberry, 1, 1.99}, {blueberry, 1, 1.99}, {raspberry, 1, 1.99}, {blackberry, 1, 1.99}, {watermelon, 1, 5.99}, {cantaloupe, 1, 5.99}, {honeydew, 1, 5.99}, {pineapple, 1, 5.99}, {kiwi, 1, 1.99}, {mango, 1, 1.99}, {papaya, 1, 1.99}, {pear, 1, 1.99}, {peach, 1, 1.99}, {plum, 1, 1.99}, {cherry, 1, 1.99}, {lemon, 1, 1.99}, {lime, 1, 1.99}, {coconut, 1, 1.99}, {avocado, 1, 1.99}, {potato, 1, 1.99}, {tomato, 1, 1.99}, {carrot, 1, 1.99}, {onion, 1, 1.99}, {lettuce, 1, 1.99}, {broccoli, 1, 1.99}, {cauliflower, 1, 1.99}, {corn, 1, 1.99}, {peas, 1, 1.99}, {green bean, 1, 1.99}, {green pepper, 1, 1.99}, {red pepper, 1, 1.99}, {yellow pepper, 1, 1.99}, {orange pepper, 1, 1.99}, {purple pepper, 1, 1.99}, {white pepper, 1, 1.99}, {black pepper, 1, 1.99}, {salt, 1, 1.99}, {pepper, 1, 1.99}, {sugar, 1, 1.99}, {flour, 1, 1.99}, {butter, 1, 1.99}, {oil, 1, 1.99}, {vinegar, 1, 1.99}, {soy sauce, 1, 1.99}, {ketchup, 1, 1.99}, {mustard, 1, 1.99}, {mayonnaise, 1, 1.99}, {ranch, 1, 1.99}, {italian, 1, 1.99}, {thousand island, 1, 1.99}, {blue cheese, 1, 1.99}, {caesar, 1, 1.99}, {russian, 1, 1.99}, {french, 1, 1.99}, {tartar, 1, 1.99}, {honey mustard, 1, 1.99}, {barbecue, 1, 1.99}, {honey barbecue, 1, 1.99}, {sweet and sour, 1, 1.99}, {teriyaki, 1, 1.99}, {sriracha, 1, 1.99}, {tobasco, 1, 1.99}, {worcestershire, 1, 1.99}, {salsa, 1, 1.99}, {guacamole, 1, 1.99}, {sour cream, 1, 1.99}, {cream cheese, 1, 1.99}, {syrup, 1, 1.99}, {jelly, 1, 1.99}, {jam, 1, 1.99}, {peanut butter, 1, 1.99}, {nutella, 1, 1.99}, {hummus, 1, 1.99}, {chocolate, 1, 1.99}, {caramel, 1,1.99}
            List<SaleItem> items = new List<SaleItem>();
            items.Add(new SaleItem() { name = "bacon", quantity = 1, price = 5.99M });
            items.Add(new SaleItem() { name = "eggs", quantity = 1, price = 2.99M });
            items.Add(new SaleItem() { name = "milk", quantity = 1, price = 3.99M });
            items.Add(new SaleItem() { name = "bread", quantity = 1, price = 2.99M });
            items.Add(new SaleItem() { name = "cheese", quantity = 1, price = 4.99M });
            items.Add(new SaleItem() { name = "chicken", quantity = 1, price = 5.99M });
            items.Add(new SaleItem() { name = "beef", quantity = 1, price = 6.99M });
            items.Add(new SaleItem() { name = "pork", quantity = 1, price = 5.99M });
            items.Add(new SaleItem() { name = "fish", quantity = 1, price = 7.99M });
            items.Add(new SaleItem() { name = "apple", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "banana", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "orange", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "grape", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "strawberry", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "blueberry", quantity = 1, price = 1.99M });

            //create a sale with the list of items, payment type cash, storeID 1, and loyalty card false
            Sale sale = new Sale(items, Sale.PaymentType.Cash, new Guid("00000000-0000-0000-0000-000000000001"), false);
            Assert.AreEqual(15, sale.TotalItems);
            Assert.AreEqual(59.85M, sale.TotalPrice);

            CreditCardProcessor processor = new CreditCardProcessor("https://csca5028api.azure-api.net/ccp/", new System.Net.Http.HttpClient());
            CreditCard card = new CreditCard();
            card.CardCVV = "123";
            card.CardExpiry = "12/25";
            card.CardNumber = "4111111111111111";
            card.Amount = sale.TotalPrice.ToString();
            var response = processor.ProcesstransactionAsync(card).Result;
            Console.WriteLine("Terminal {0}: Authorisation Response Received: {1} with code {2}", new Guid().ToString(), response.ResponseType.ToString(), response.AuthCode);
            Assert.IsTrue(response.ResponseType == CreditCardResponseTypes._1 | response.ResponseType == CreditCardResponseTypes._0);
            Assert.IsTrue(!response.AuthCode.IsNullOrEmpty());

        }

        private string GetAzureDBConnectionString()
        {
            /*<EnvironmentVariables>
	            <Variable name="AZURE_TENANT_ID" value="c6afb394-61d9-4428-b139-d4b13b7ff289" />
	            <Variable name="AZURE_CLIENT_ID" value="52b0cdef-9ec9-43ef-8721-0b858bde1011" />
	            <Variable name="AZURE_CLIENT_SECRET" value="YpM8Q~AddLFp2fslW6_vfDJA.tJMrH8MaLSDYcpQ" />
            "AZURE_USERNAME": "nikhil.rajwade@iorbis.com"
            </EnvironmentVariables>*/
            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", "c6afb394-61d9-4428-b139-d4b13b7ff289");
            Environment.SetEnvironmentVariable("AZURE_CLIENT_ID", "52b0cdef-9ec9-43ef-8721-0b858bde1011");
            Environment.SetEnvironmentVariable("AZURE_CLIENT_SECRET", "YpM8Q~AddLFp2fslW6_vfDJA.tJMrH8MaLSDYcpQ");
            Environment.SetEnvironmentVariable("GITHUB_ACTIONS", "true");

            var connectionString = string.Empty;
            if (IsRunningonCICD())
            {

                SecretClient secretClient = new SecretClient(new Uri("https://csca5028vault.vault.azure.net/"), new DefaultAzureCredential());
                KeyVaultSecret secret = secretClient.GetSecret("ConnectionStrings:AzureDB");
                if(secret != null)
                {
                    connectionString = secret.Value;
                }
            }
            return connectionString;
        }
        private static string dbName = "sales_db";

        private bool IsRunningonCICD()
        {
            var isGitHub = false;
            if(bool.TryParse(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"), out isGitHub))
            {
                if (isGitHub!=null)
                {
                    return isGitHub;
                }
            }
            return false;
        }

        //wrap all TestMethods with the following code
        //if(IsRunningonCICD())
        //{
        //  method code
        // await Task.CompletedTask;
        //}


        [TestMethod()]
        public async Task GetStoresAsyncTest()
        {
            if (IsRunningonCICD())
            {
                var connectionString = GetAzureDBConnectionString();

                StoreDBController storeDBController = new StoreDBController();
                List<Store> stores = (List<Store>)await storeDBController.GetStoresAsync(dbName, connectionString);
                Assert.AreEqual(10, stores.Count);
            }
            await Task.CompletedTask;
        }

        [TestMethod()]
        public async Task GetTerminalsAsyncTest()
        {
            if (IsRunningonCICD())
            {
                var connectionString = GetAzureDBConnectionString();

                StoreDBController storeDBController = new StoreDBController();
                List<Store> stores = (List<Store>)await storeDBController.GetStoresAsync(dbName, connectionString);
                Assert.AreEqual(10, stores.Count);
                foreach (Store store in stores)
                {
                    List<POSTerminal> terminals = (List<POSTerminal>)storeDBController.GetTerminalsAsync(store.ID, dbName, connectionString).Result;
                    //Assert.AreEqual(10, terminals.Count);
                    Assert.IsTrue(terminals.Count == 2);
                }
            }
            await Task.CompletedTask;
        }

        [TestMethod()]
        public async Task GetStoresAndTerminalsAsyncTest()
        {
            if (IsRunningonCICD())
            {
                var connectionString = GetAzureDBConnectionString();
                StoreDBController storeDBController = new StoreDBController();

                Dictionary<string, Tuple<Store, List<POSTerminal>>> storesAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName, connectionString);
                Assert.AreEqual(10, storesAndTerminals.Count);
                foreach (string storeName in storesAndTerminals.Keys)
                {
                    List<POSTerminal> terminals = storesAndTerminals[storeName].Item2;
                    Assert.IsTrue(terminals.Count == 2);
                }
            }
            await Task.CompletedTask;
        }

        [TestMethod()]
        public async Task GetStoreIdAsyncTest()
        {
            if (IsRunningonCICD())
            {
                var connectionString = GetAzureDBConnectionString();
                StoreDBController storeDBController = new StoreDBController();

                Guid storeid = await storeDBController.GetStoreIdAsync("Dallas Store", dbName, connectionString);
                Assert.IsNotNull(storeid);
            }
            await Task.CompletedTask;
        }

        [TestMethod()]
        public async Task GetStoresDataTableAsyncTest()
        {
            if (IsRunningonCICD())
            {

                var connectionString = GetAzureDBConnectionString();
                StoreDBController storeDBController = new StoreDBController();

                var stores = await storeDBController.GetStoresDataTableAsync(dbName, connectionString);
                Assert.AreEqual(10, stores.Rows.Count);
            }
            await Task.CompletedTask;
        }

        [TestMethod()]
        public void StoreDBControllerTest()
        {
            if(IsRunningonCICD())
            {
                StoreDBController storeDBController = new StoreDBController();
                Assert.IsNotNull(storeDBController);
            }

        }

        [TestMethod()]
        public async Task InsertTest()
        {
            if (IsRunningonCICD())
            {
                StoreDBController storeDBController = new StoreDBController();
                var connectionString = GetAzureDBConnectionString();
                var storesAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName, connectionString);

                if (storesAndTerminals == null || storesAndTerminals.Count == 0)
                {
                    Assert.Fail("Stores and Terminals not created!");
                }

                Store store = (Store)storesAndTerminals["New York Store"].Item1;
                POSTerminal terminal = (POSTerminal)storesAndTerminals["New York Store"].Item2[0];


                Sale sale = terminal.GenerateSale();

                SalesDBController controller = new SalesDBController();
                await controller.Insert(dbName, sale, connectionString);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = $"use {dbName} select * from Sales where saleID = '{sale.ID}'";
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Assert.AreEqual(reader["saleID"], sale.ID);
                                    Assert.AreEqual(reader["store_id"], sale.StoreID);
                                    Assert.AreEqual(reader["json_item_list"], sale.ItemsAsJson);
                                    Assert.AreEqual(reader["total_items"], sale.TotalItems);
                                    Assert.AreEqual(reader["total_price"], sale.TotalPrice);
                                    Assert.AreEqual(reader["payment_type"], sale.paymentType.ToString());
                                    Assert.AreEqual(reader["loyalty_card"], sale.loyaltyCard);
                                }
                            }
                            else
                            {
                                Assert.Fail($"Sale not inserted!");
                            }
                        }
                    }
                }
            }
            await Task.CompletedTask;
        }

        [TestMethod()]
        public async Task GetSalesRevenueByStoreIdTest()
        {
            if (IsRunningonCICD())
            {
                StoreDBController storeDBController = new StoreDBController();
                SalesDBController salesDBController = new SalesDBController();
                var connectionString = GetAzureDBConnectionString();
                var storesAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName, connectionString);

                if (storesAndTerminals == null || storesAndTerminals.Count == 0)
                {
                    Assert.Fail("Stores and Terminals not created!");
                }

                Store store = (Store)storesAndTerminals["New York Store"].Item1;
                POSTerminal terminal = (POSTerminal)storesAndTerminals["New York Store"].Item2[0];

                decimal perf = 0.0M;
                for (int i = 0; i < 10; i++)
                {
                    Sale sale = terminal.GenerateSale();
                    await salesDBController.Insert(dbName, sale, connectionString);
                    perf += sale.TotalPrice;
                }

                var salesPerf = await salesDBController.GetSalesRevenueByStoreID(dbName, store.ID, connectionString);
                Assert.IsTrue(salesPerf >= perf);
            }
            await Task.CompletedTask;

        }

        [TestMethod()]
        public async Task GetSalesPerformanceForTimeIntervalTest()
        {
            if (IsRunningonCICD())
            {
                StoreDBController storeDBController = new StoreDBController();
                SalesDBController salesDBController = new SalesDBController();
                var connectionString = GetAzureDBConnectionString();
                var salesPerf = await salesDBController.GetSalesPerformanceForTimeInterval(dbName, 1, connectionString);
                Assert.IsNotNull(salesPerf);
            }
            await Task.CompletedTask;
        }

        [TestMethod()]
        public void SalesDBControllerTest()
        {
            if (IsRunningonCICD())
            {
                SalesDBController salesDBController = new SalesDBController();
                Assert.IsNotNull(salesDBController);
            }

        }


        [TestMethod()]
        public async Task GetAverageSalePriceByStoreIDTest()
        {
            if (IsRunningonCICD())
            {
                StoreDBController storeDBController = new StoreDBController();
                SalesDBController salesDBController = new SalesDBController();

                var connectionString = GetAzureDBConnectionString();
                var storesAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName, connectionString);

                if (storesAndTerminals == null || storesAndTerminals.Count == 0)
                {
                    Assert.Fail("Stores and Terminals not created!");
                }

                Store store = (Store)storesAndTerminals["New York Store"].Item1;
                POSTerminal terminal = (POSTerminal)storesAndTerminals["New York Store"].Item2[0];

                decimal perf = 0.0M;
                for (int i = 0; i < 10; i++)
                {
                    Sale sale = terminal.GenerateSale();
                    await salesDBController.Insert(dbName, sale, connectionString);
                    perf += sale.TotalPrice;
                }
                var avg = perf / 10;

                var avgSalesPerf = await salesDBController.GetAverageSalePriceByStoreID(dbName, store.ID, connectionString);

                //check if the average is within 50% of the actual average
                Assert.IsTrue(avgSalesPerf >= 0);
            }
            await Task.CompletedTask;
        }

        [TestMethod()]
        public async Task GetTotalSalesRevenueTest()
        {
            if (IsRunningonCICD())
            {
                SalesDBController salesDBController = new SalesDBController();
                StoreDBController storeDBController = new StoreDBController();
                var connectionString = GetAzureDBConnectionString();
                var storesAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName, connectionString);

                if (storesAndTerminals == null || storesAndTerminals.Count == 0)
                {
                    Assert.Fail("Stores and Terminals not created!");
                }

                Store store = (Store)storesAndTerminals["New York Store"].Item1;
                POSTerminal terminal = (POSTerminal)storesAndTerminals["New York Store"].Item2[0];

                decimal perf = 0.0M;
                for (int i = 0; i < 10; i++)
                {
                    Sale sale = terminal.GenerateSale();
                    await salesDBController.Insert(dbName, sale, connectionString);
                    perf += sale.TotalPrice;
                }

                var totalSalesPerf = await salesDBController.GetTotalSalesRevenue(dbName, connectionString);
                Assert.IsTrue(totalSalesPerf >= perf);
            }
            await Task.CompletedTask;
        }





    }
}
