using Microsoft.AspNetCore.Mvc;
using WarehouseWeb2.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using Warehouseweb2.Models;

namespace Warehouseweb2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var resultList = await _dbContext.RawSqlQuery<OverviewDetails>(
                $@"
                Select total , capacity , round((total/capacity)*100,0) as usage  , incoming, outgoing
                from 
                (Select sum(capacity) as capacity from location) a,
                (Select sum(expected_quantity) as incoming from incoming where status = 'Pending' ) b,
                (Select Sum(quantity) as total from inventory) c ,
                (Select sum(ordered_quantity) as outgoing from outgoing where status = 'Pending') d
                ",
            reader => new OverviewDetails
            {
                total = reader.GetInt32(reader.GetOrdinal("total")),
                capacity = reader.GetInt32(reader.GetOrdinal("capacity")),
                usage = reader.GetDouble(reader.GetOrdinal("usage")),
                incoming = reader.GetInt32(reader.GetOrdinal("incoming")),
                outgoing = reader.GetInt32(reader.GetOrdinal("outgoing"))
            }
            );

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

            var ProductList = await _dbContext.RawSqlQuery<Top5Product>(
                $@"
                Select t2.category,t2.product_name, t1.quantity, t3.location_name, to_char(t1.last_updated,'DD-MON-YY') as last_update
                from inventory t1
                left join product t2 on t1.product_id = t2.product_id
                left join location t3 on t1.location_id = t3.location_id
                order by t1.quantity desc
                LIMIT 5
                ",
            reader => new Top5Product
            {
                category = reader.GetString(reader.GetOrdinal("category")),
                product_name = reader.GetString(reader.GetOrdinal("product_name")),
                quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                location_name = reader.GetString(reader.GetOrdinal("location_name")),
                last_update = reader.GetString(reader.GetOrdinal("last_update"))
            }
            );

            var vm = new OverviewDetailsView
            {
                OverviewDetails = resultList.FirstOrDefault(),
                Area = AreaList,
                Top5Product = ProductList
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}