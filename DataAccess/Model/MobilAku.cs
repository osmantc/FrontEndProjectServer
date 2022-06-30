using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DataAccess.Model
{
    public class MobilAku
    {
        public int id { get; set; }
        public string tenant_id { get; set; }
        public int? report_file_process_id { get; set; }

        public string location { get; set; }
        public string asset_num { get; set; }
        public int n_of_ac { get; set; }
        public int n_of_ne { get; set; }
        public string battery_age { get; set; }
        public int n_of_partial_charge { get; set; }
        public int n_of_generator { get; set; }
        public int n_of_air_con { get; set; }
        public string max_ac_duration { get; set; }
        public int mx_afad { get; set; }
        public string totalpower_loc { get; set; }
        public string ideal_working_hour { get; set; }
        public string back_sites { get; set; }
        public string technology { get; set; }
        public string remaining_battery_lifetime { get; set; }
        public int remaining_battery_lifetime_cast_int { get; set; }
        public string recommendation { get; set; }
        public string additional_info { get; set; }
        public DateTime current_date { get; set; }


        public int RegionsId { get; set; }
        public Regions Regions { get; set; }
        public int CitiesId { get; set; }
        public Cities Cities { get; set; }

    }
}