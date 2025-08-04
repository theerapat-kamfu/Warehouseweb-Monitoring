using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouseweb2.Models
{
    public class OverviewDetailsView
    {
        public OverviewDetails OverviewDetails { get; set; }

        public List<Area> Area { get; set; }

        public List<Top5Product> Top5Product { get; set; }

        public LocationDetails LocationDetails { get; set; }

    }
}