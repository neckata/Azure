using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Functions.Models;
using System.Data.SqlClient;

namespace Functions.Functions
{
    public static class SQLFunction
    {
        [FunctionName("DatabaseFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Connecting to SQL Database");

            //string _conn_string = "Server=tcp:demoserver4000.database.windows.net,1433;Initial Catalog=demodb;Persist Security Info=False;User ID=demousr;Password=Shakinstev123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string _conn_string = Environment.GetEnvironmentVariable("sql_connection");

            List<Product> _products = new List<Product>();
            using (SqlConnection _connection = new SqlConnection(_conn_string))
            {
                _connection.Open();
                string _query = "select Id,Name,price from Products";


                using (SqlCommand _cmd = new SqlCommand(_query, _connection))
                {
                    SqlDataReader _reader = _cmd.ExecuteReader();
                    while (_reader.Read())
                    {
                        Product obj = new Product();
                        obj.Id = _reader.GetInt32(0);
                        obj.Name = _reader.GetString(1);
                        obj.price = _reader.GetFloat(2);
                        _products.Add(obj);
                    }
                }
            }

            return new OkObjectResult(_products);
        }
    }
}
