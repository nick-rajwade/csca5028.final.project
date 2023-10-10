using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace csca5028.final.project.components
{
    public class SalesDBController
    {
        public async Task Insert(string dbName, Sale sale, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString)) 
            { 
                await conn.OpenAsync();
                var sql = @$"INSERT INTO [{dbName}].[dbo].[Sales] ([saleId], [store_id], [loyalty_card], [payment_type], [total_items], [total_price], [json_item_list], [created_at], [CC_AUTH], [CC_AUTH_CODE])
                    VALUES (@saleId, @store_id, @loyalty_card, @payment_type, @total_items, @total_price, @json_item_list, @created_at, @CC_AUTH, @CC_AUTH_CODE);";
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@saleId", sale.ID);
                    cmd.Parameters.AddWithValue("@store_id", sale.StoreID);
                    cmd.Parameters.AddWithValue("@loyalty_card", sale.loyaltyCard);
                    cmd.Parameters.AddWithValue("@payment_type", sale.paymentType.ToString());
                    cmd.Parameters.AddWithValue("@total_items", sale.TotalItems);
                    cmd.Parameters.AddWithValue("@total_price", sale.TotalPrice);
                    cmd.Parameters.AddWithValue("@json_item_list", sale.ItemsAsJson);
                    cmd.Parameters.AddWithValue("@created_at", sale.CreatedAt);
                    if(sale.CreditCardResponse != null)
                    {
                        if(sale.CreditCardResponse.ResponseType == CreditCardResponseTypes._0)
                        {
                            cmd.Parameters.AddWithValue("@CC_AUTH", "AUTH");
                            cmd.Parameters.AddWithValue("@CC_AUTH_CODE", sale.CreditCardResponse?.AuthCode);

                        }
                        else if(sale.CreditCardResponse.ResponseType == CreditCardResponseTypes._1)
                        {
                            cmd.Parameters.AddWithValue("@CC_AUTH", "DECLINE");
                            cmd.Parameters.AddWithValue("@CC_AUTH_CODE", sale.CreditCardResponse?.AuthCode);
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CC_AUTH", DBNull.Value);
                        cmd.Parameters.AddWithValue("@CC_AUTH_CODE", DBNull.Value);
                    }

                    await cmd.ExecuteNonQueryAsync();
                }


            }
        }

        //from sales_db get all sales for a given store and calculate the sales performance
        public async Task<decimal> GetSalesRevenueByStoreID(string dbName, Guid storeId, string connectionString)
        {
            decimal salesPerformance = 0;
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var sql = @$"SELECT [total_price] FROM [{dbName}].[dbo].[Sales] WHERE [store_id] = @storeId AND (CC_AUTH IS NULL OR CC_AUTH <> 'DECLINE');";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@storeId", storeId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            salesPerformance += reader.GetDecimal(0);
                        }
                    }
                }
            }
            return salesPerformance;
        }

        //Get average sale price for a given store
        public async Task<decimal> GetAverageSalePriceByStoreID(string dbName, Guid storeId, string connectionString)
        {
            decimal averageSalePrice = 0;
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var sql = @$"SELECT AVG([total_price]) FROM [{dbName}].[dbo].[Sales] WHERE [store_id] = @storeId AND (CC_AUTH IS NULL OR CC_AUTH <> 'DECLINE');";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@storeId", storeId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            averageSalePrice = reader.GetDecimal(0);
                        }
                    }
                }
            }
            return averageSalePrice;
        }

        //get sales per minute for a given store
        public async Task<Dictionary<string,Tuple<int,decimal>>> GetSalesPerformanceForTimeInterval(string dbName, int timeIntervalMinutes, string connectionString)
        {
			DataSet ds = new DataSet();
            using (var conn = new SqlConnection(connectionString))
            {
				await conn.OpenAsync();
                //var sql = @$"SELECT store_id, COUNT(*) as txnCount, sum(total_price) as total_sales FROM [{dbName}].[dbo].[Sales] WHERE created_at > DATEADD(MINUTE,@timeIntervalMinutes,GETDATE()) AND (CC_AUTH IS NULL OR CC_AUTH <> 'DECLINE') group by store_id;";
                var sql = $@"SELECT t.store_name as name, COUNT(*) as txn_count, SUM(total_price) as total_sales FROM [{dbName}].[dbo].[Sales] s
                            INNER JOIN [{dbName}].[dbo].[Stores] t ON s.store_id = t.id
                            WHERE s.created_at > DATEADD(MINUTE,@timeIntervalMinutes,GETDATE())
                            AND (s.CC_AUTH IS NULL OR s.CC_AUTH <> 'DECLINE')
                            GROUP BY s.store_id, t.store_name;";
				
                
                using (var cmd = conn.CreateCommand())
                {
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@timeIntervalMinutes", 0-timeIntervalMinutes);
					using(SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
						da.Fill(ds, "SalesSummary");
					}
				}
			}
            Dictionary<string, Tuple<int, decimal>> salesPerformance = ds.Tables["SalesSummary"].AsEnumerable()
                .ToDictionary<DataRow, string, Tuple<int, decimal>>(row =>
                row.Field<string>(0), row => new Tuple<int, decimal>(row.Field<int>(1), row.Field<decimal>(2)));

            
			return salesPerformance;
		}

        public async Task<decimal> GetTotalSalesRevenue(string dbName, string connectionString)
        {
            decimal revenue = 0M;
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var sql = @$"SELECT SUM(total_price) FROM [{dbName}].[dbo].[Sales] WHERE (CC_AUTH IS NULL OR CC_AUTH <> 'DECLINE');";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            revenue = reader.GetDecimal(0);
                        }
                    }
                }
            }
            return revenue;
        }

    }
}
