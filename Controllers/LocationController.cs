using Microsoft.AspNetCore.Mvc;
using WarehouseWeb2.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using Warehouseweb2.Models;
using System.Linq;

namespace Warehouseweb2.Controllers
{
    public class LocationController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public LocationController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Location()
        {
            var AreaList = await _dbContext.RawSqlQuery<Area>(
                $@"
                Select t1.location_id,t2.location_name,round((t1.qty/t2.capacity)*100,0) as usage
                from(
                    Select location_id, sum(quantity) as qty
                    from inventory
                    group by location_id) t1 
                left join (
                    Select location_id,location_name,capacity from location) t2 
                on t1.location_id = t2.location_id
                ",
                reader => new Area
                {
                    location_id = reader.GetInt32(reader.GetOrdinal("location_id")),
                    location_name = reader.GetString(reader.GetOrdinal("location_name")),
                    usage = reader.GetDouble(reader.GetOrdinal("usage"))
                }
            );

            var vm = new OverviewDetailsView
            {
                Area = AreaList,
            };

            return View(vm);
        }
        
        public async Task<IActionResult> Details(int locationId)
        {
            var sqlQuery = $@"
                WITH inven AS (
                    SELECT location_id, SUM(quantity) AS quantity
                    FROM inventory
                    GROUP BY location_id
                    ORDER BY location_id 
                ), 
                loca AS (
                    SELECT location_id, location_name, capacity, unit_of_capacity AS unit
                    FROM location
                )
                SELECT t1.location_id, t2.location_name, t1.quantity, t2.capacity, t2.unit
                FROM inven t1
                LEFT JOIN loca t2 ON t1.location_id = t2.location_id
                WHERE t1.location_id = {locationId}
                ";

            var locationDetails = await _dbContext.RawSqlQuery<LocationDetails>(
                sqlQuery, 
                reader => new LocationDetails
                {
                    location_id = reader.GetInt32(reader.GetOrdinal("location_id")),
                    location_name = reader.GetString(reader.GetOrdinal("location_name")),
                    quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                    capacity = reader.GetInt32(reader.GetOrdinal("capacity")),
                    unit = reader.GetString(reader.GetOrdinal("unit"))
                }

            );

            var detail = locationDetails.FirstOrDefault();

            if (detail == null)
            {
                return NotFound(new { message = $"ไม่พบข้อมูลรายละเอียดสำหรับ Location ID: {locationId}" }); 
            }

            var vm = new OverviewDetailsView
            {
                LocationDetails = detail
            };

            return View(vm);
        }
    }
}