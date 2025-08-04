using Microsoft.AspNetCore.Mvc;
using WarehouseWeb2.Data;
using Warehouseweb2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using System;
using System.Linq; 

namespace Warehouseweb2.Controllers
{
    public class OutgoingController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public OutgoingController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Outgoing()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetPaginatedOutgoingData(
            int pageNumber = 1,
            int pageSize = 6)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 1;
            if (pageSize > 100) pageSize = 100;

            int offset = (pageNumber - 1) * pageSize;

            string query = @"
                SELECT
                    t1.product_id,
                    t2.product_name,
                    t2.category,
                    t1.scheduled_ship_date,
                    t1.ordered_quantity,
                    t1.customer_name,
                    t1.status,
                    COUNT(*) OVER() as total_records
                FROM outgoing t1
                LEFT JOIN product t2 ON t1.product_id = t2.product_id
                ORDER BY t1.scheduled_ship_date DESC
                OFFSET @offset LIMIT @pageSize;";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("@offset", offset),
                new NpgsqlParameter("@pageSize", pageSize)
            };

            List<OutgoingItem> outgoingItems = new List<OutgoingItem>();
            int totalRecords = 0;

            try
            {
                var result = await _dbContext.RawSqlQuery(
                    query,
                    reader => new
                    {
                        Item = new OutgoingItem
                        {
                            ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
                            ProductName = reader.IsDBNull(reader.GetOrdinal("product_name")) ? null : reader.GetString(reader.GetOrdinal("product_name")),
                            Category = reader.IsDBNull(reader.GetOrdinal("category")) ? null : reader.GetString(reader.GetOrdinal("category")),
                            ScheduledShipDate = reader.IsDBNull(reader.GetOrdinal("scheduled_ship_date"))
                                ? null
                                : reader.GetDateTime(reader.GetOrdinal("scheduled_ship_date")).ToString("yyyy-MM-dd"),
                            OrderedQuantity = reader.GetInt32(reader.GetOrdinal("ordered_quantity")),
                            CustomerName = reader.IsDBNull(reader.GetOrdinal("customer_name")) ? null : reader.GetString(reader.GetOrdinal("customer_name")),
                            Status = reader.GetString(reader.GetOrdinal("status"))
                        },
                        TotalRecords = reader.GetInt32(reader.GetOrdinal("total_records"))
                    },
                    parameters.ToArray()
                );
                
                if (result.Count > 0)
                {
                    totalRecords = result[0].TotalRecords;
                    foreach (var record in result)
                    {
                        outgoingItems.Add(record.Item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching paginated outgoing data: {ex.Message}");
                return Json(new { error = "An error occurred while fetching data.", message = ex.Message });
            }

            return Json(new {
                data = outgoingItems,
                totalRecords = totalRecords,
                pageNumber = pageNumber,
                pageSize = pageSize
            });
        }
    }
}