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
    public class IncomingController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public IncomingController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Incoming()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetPaginatedIncomingData(
            int pageNumber = 1,
            int pageSize = 6)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 1;
            if (pageSize > 100) pageSize = 100;

            int offset = (pageNumber - 1) * pageSize;

            string baseQuery = @"
                FROM incoming t1
                LEFT JOIN product t2 ON t1.product_id = t2.product_id";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            string countQuery = $"SELECT COUNT(*) {baseQuery}";
            int totalRecords = 0;
            try
            {
                totalRecords = await _dbContext.RawScalarQuery<int>(countQuery, parameters.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting records: {ex.Message}");
            }

            string dataQuery = $@"
                SELECT
                    t1.product_id,
                    t2.product_name,
                    t2.category,
                    t1.supplier_name,
                    t1.expected_quantity,
                    t1.expected_arrival_date,
                    t1.status
                {baseQuery}
                ORDER BY t1.expected_arrival_date DESC
                OFFSET @offset LIMIT @pageSize;";

            parameters.Add(new NpgsqlParameter("@offset", offset));
            parameters.Add(new NpgsqlParameter("@pageSize", pageSize));

            List<IncomingItem> incomingItems = new List<IncomingItem>();

            try
            {
                incomingItems = await _dbContext.RawSqlQuery<IncomingItem>(
                    dataQuery,
                    reader => new IncomingItem
                    {
                        ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
                        ProductName = reader.IsDBNull(reader.GetOrdinal("product_name")) ? null : reader.GetString(reader.GetOrdinal("product_name")),
                        Category = reader.IsDBNull(reader.GetOrdinal("category")) ? null : reader.GetString(reader.GetOrdinal("category")),
                        SupplierName = reader.GetString(reader.GetOrdinal("supplier_name")),
                        ExpectedQuantity = reader.GetInt32(reader.GetOrdinal("expected_quantity")),
                        ExpectedArrivalDate = reader.IsDBNull(reader.GetOrdinal("expected_arrival_date"))
                                                ? null
                                                : reader.GetDateTime(reader.GetOrdinal("expected_arrival_date")).ToString("yyyy-MM-dd"),
                        Status = reader.GetString(reader.GetOrdinal("status"))
                    },
                    parameters.ToArray()
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching paginated incoming data: {ex.Message}");
                return Json(new { error = "An error occurred while fetching data.", message = ex.Message });
            }

            return Json(new {
                data = incomingItems,
                totalRecords = totalRecords,
                pageNumber = pageNumber,
                pageSize = pageSize
            });
        }
    }
}