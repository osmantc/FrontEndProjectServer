using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DataAccess.Model
{
    public class Regions
    {
        public Regions()
        {
            Cities = new HashSet<Cities>();
            MobilAku = new HashSet<MobilAku>();
        }

        public int id { get; set; }
        public string name { get; set; }

        public ICollection<Cities> Cities { get; set; }

        public ICollection<MobilAku> MobilAku { get; set; }
    }
}