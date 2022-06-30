using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DataAccess.Model
{
    public class Cities
    {
        public Cities()
        {
            MobilAku = new HashSet<MobilAku>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public int RegionsId { get; set; }
        public Regions Regions { get; set; }


        public ICollection<MobilAku> MobilAku { get; set; }
    }
}