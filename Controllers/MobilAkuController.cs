using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.DataAccess;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Server.DataAccess.Model;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;


using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;

namespace Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MobilAkuController : Controller
    {
        private MyDbContext _context { get; set; }
        private ILogger<MobilAkuController> _logger;

        public MobilAkuController(MyDbContext context, ILogger<MobilAkuController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> MobilAkus(DataSourceLoadOptions loadOptions)
        {
            try
            {
                _logger.LogInformation("çalıştım.");
                return Json(await DataSourceLoader.LoadAsync(_context.MobilAku.Include(x => x.Cities).Include(x => x.Regions), loadOptions));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"hata {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertMobilAku(string values)
        {
            var mobilAku = new MobilAku();

            //FIXME:Burası patlarsa manuel PopulateModel nesnesi yazılıp yapılacak
            // JsonConvert.PopulateObject(values, mobilAku);

            PopulateModel(mobilAku, JsonConvert.DeserializeObject<IDictionary>(values));

            if (!TryValidateModel(mobilAku))
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));

            var result = _context.MobilAku.Add(mobilAku);
            await _context.SaveChangesAsync();

            return Ok(result.Entity);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMobilAku(int key, string values)
        {
            var mobilAku = await _context.MobilAku.Include(x => x.Cities).Include(x => x.Regions).FirstOrDefaultAsync(o => o.id == key);

            //FIXME:Burası patlarsa manuel PopulateModel nesnesi yazılıp yapılacak
            // JsonConvert.PopulateObject(values, mobilAku);

            PopulateModel(mobilAku, JsonConvert.DeserializeObject<IDictionary>(values));

            //TODO:Test modundan, Production üretimine geçerken architectural ve mantıksal (cities boş işe vs.) ve performans (her seferinde gitmesine gerek yok vb.) olarak zaten burası düzenlenecektir.
            //TODO:Ileride city ve region updateleri için


            if (!TryValidateModel(mobilAku))
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));

            await _context.SaveChangesAsync();
            return Ok(mobilAku);
        }

        [HttpDelete]
        public async Task DeleteMobilAku(int key)
        {
            var mobilAku = await _context.MobilAku.FirstAsync(o => o.id == key);
            _context.MobilAku.Remove(mobilAku);
            await _context.SaveChangesAsync();
        }


        //*Additional endpoints
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var mobilAkus = await _context.MobilAku.ToListAsync();

                return Ok(mobilAkus);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var mobilAku = await _context.MobilAku.FindAsync(id);

                return Ok(mobilAku);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //TODO:Dökümantasyona göre (bir sıkıntı çıkmasın düzgün çalıştığını göreyim) şimdilik bu şekilde, ileride direkt AutoMapper ile.
        private void PopulateModel(MobilAku mobilAku, IDictionary values)
        {
            if (values.Contains("id"))
                mobilAku.id = Convert.ToInt32(values["id"]);
            if (values.Contains("tenant_id"))
                mobilAku.tenant_id = (values["tenant_id"] != null) ? values["tenant_id"].ToString() : string.Empty;
            if (values.Contains("report_file_process_id"))
                mobilAku.report_file_process_id = Convert.ToInt32(values["report_file_process_id"]);
            if (values.Contains("location"))
                mobilAku.location = (values["location"] != null) ? values["location"].ToString() : string.Empty;
            if (values.Contains("asset_num"))
                mobilAku.asset_num = (values["asset_num"] != null) ? values["asset_num"].ToString() : string.Empty;
            if (values.Contains("n_of_ac"))
                mobilAku.n_of_ac = Convert.ToInt32(values["n_of_ac"]);
            if (values.Contains("n_of_ne"))
                mobilAku.n_of_ne = Convert.ToInt32(values["n_of_ne"]);
            if (values.Contains("battery_age"))
                mobilAku.battery_age = (values["battery_age"] != null) ? values["battery_age"].ToString() : string.Empty;
            if (values.Contains("n_of_partial_charge"))
                mobilAku.n_of_partial_charge = Convert.ToInt32(values["n_of_partial_charge"]);
            if (values.Contains("n_of_generator"))
                mobilAku.n_of_generator = Convert.ToInt32(values["n_of_generator"]);
            if (values.Contains("n_of_air_con"))
                mobilAku.n_of_air_con = Convert.ToInt32(values["n_of_air_con"]);
            if (values.Contains("max_ac_duration"))
                mobilAku.max_ac_duration = (values["max_ac_duration"] != null) ? values["max_ac_duration"].ToString() : string.Empty;
            if (values.Contains("mx_afad"))
                mobilAku.mx_afad = Convert.ToInt32(values["mx_afad"]);
            if (values.Contains("totalpower_loc"))
                mobilAku.totalpower_loc = (values["totalpower_loc"] != null) ? values["totalpower_loc"].ToString() : string.Empty;
            if (values.Contains("ideal_working_hour"))
                mobilAku.ideal_working_hour = (values["ideal_working_hour"] != null) ? values["ideal_working_hour"].ToString() : string.Empty;
            if (values.Contains("back_sites"))
                mobilAku.back_sites = (values["back_sites"] != null) ? values["back_sites"].ToString() : string.Empty;
            if (values.Contains("technology"))
                mobilAku.technology = (values["technology"] != null) ? values["technology"].ToString() : string.Empty;
            if (values.Contains("remaining_battery_lifetime"))
                mobilAku.remaining_battery_lifetime = (values["remaining_battery_lifetime"] != null) ? values["remaining_battery_lifetime"].ToString() : string.Empty;
            if (values.Contains("remaining_battery_lifetime_cast_int"))
                mobilAku.remaining_battery_lifetime_cast_int = Convert.ToInt32(values["remaining_battery_lifetime_cast_int"]);
            if (values.Contains("recommendation"))
                mobilAku.recommendation = (values["recommendation"] != null) ? values["recommendation"].ToString() : string.Empty;
            if (values.Contains("additional_info"))
                mobilAku.additional_info = (values["additional_info"] != null) ? values["additional_info"].ToString() : string.Empty;
            if (values.Contains("current_date"))
                mobilAku.current_date = values["current_date"] != null ? Convert.ToDateTime(values["current_date"]) : new DateTime(0, 0, 0);
            if (values.Contains("CitiesId"))
                mobilAku.CitiesId = Convert.ToInt32(values["CitiesId"]);
            if (values.Contains("RegionsId"))
                mobilAku.RegionsId = Convert.ToInt32(values["RegionsId"]);
        }


    }
}