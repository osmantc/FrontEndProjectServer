using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.DataAccess;
using FrontEndProjectServer.DataAccess.ViewModel;
using DevExtreme.AspNet.Data;
// using DevExtreme.AspNet.Mvc;
using Server.DataAccess.Model;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Collections;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using FrontEndProjectServer.Helpers;

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

        //! Testte tek bir ViewModel kullandığımdan (test olduğu için) tüm propertyler gidiyor, ileride sadece aküVM ile gerekli olan (ilk etapta gösterilen 5 property) yollanacak.
        //! şimdilik hepsini yolluyorum.
        [HttpGet]
        public async Task<IActionResult> MobilAkus(DataSourceLoadOptions loadOptions)
        {
            try
            {
                _logger.LogInformation("çalıştım.");
                bool checker = false;

                if (loadOptions.Filter is not null && loadOptions.Filter.Count > 0)
                {
                    for (int i = 0; i < loadOptions.Filter.Count; i++)
                    {
                        if (loadOptions.Filter[i].ToString() == "regionsName")
                            loadOptions.Filter[i] = "Regions.name";
                        else if (loadOptions.Filter[i].ToString() == "citiesName")
                            loadOptions.Filter[i] = "Cities.name";
                    }

                }

                if (loadOptions.Sort is not null && loadOptions.Sort.Any())
                {
                    foreach (var item in loadOptions.Sort)
                    {
                        if (item.Selector == "regionsName")
                            item.Selector = "Regions.name";
                        else if (item.Selector == "citiesName")
                            item.Selector = "Cities.name";
                    }
                }

                //TODO: gruplamayı arayüzden şimdilik kaldırdım.
                if (loadOptions.Group is not null && loadOptions.Group.Any())
                {
                    foreach (var item in loadOptions.Group)
                    {
                        if (item.Selector == "regionsName")
                            item.Selector = "Regions.name";
                        else if (item.Selector == "citiesName")
                            item.Selector = "Cities.name";
                    }

                }

                var result = await DataSourceLoader.LoadAsync(_context.MobilAku.Include(x => x.Cities).Include(x => x.Regions), loadOptions);

                //TODO: ilerde performans için listeyi result.TotalCount ile kur ve test et, şimdilik böyle
                List<MobilAkuVM> newData = new List<MobilAkuVM>();

                foreach (var item in result.data)
                {
                    if (item is not MobilAku)
                    {
                        checker = true;
                        break;
                    }

                    MobilAku akuNesne = (MobilAku)item;
                    MobilAkuVM vm = new MobilAkuVM();
                    PopulateVM(vm, akuNesne);
                    newData.Add(vm);
                }

                if (checker == false)
                    result.data = newData;

                return Json(result);
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

            var convertedValues = JsonConvert.DeserializeObject<IDictionary>(values);

            PopulateModel(mobilAku, convertedValues);

            //TODO: DevExpress componenti TryValidate'yi forceladığı için relational eklemeleri şimdilik hızlıca bu şekilde tek tek ve include ile yaptım. ilerde düzeltilecek. Gerekirse tryValidate kullanmayız.

            if (convertedValues.Contains("citiesId"))
                mobilAku.Cities = await _context.Cities.Include(x => x.Regions).FirstOrDefaultAsync(x => x.id == Convert.ToInt32(convertedValues["citiesId"]));
            else
                mobilAku.Cities = await _context.Cities.Include(x => x.Regions).FirstOrDefaultAsync(x => x.id == mobilAku.CitiesId);
            if (convertedValues.Contains("regionsId"))
                mobilAku.Regions = await _context.Regions.Include(x => x.Cities).FirstOrDefaultAsync(x => x.id == Convert.ToInt32(convertedValues["regionsId"]));
            else
                mobilAku.Regions = await _context.Regions.Include(x => x.Cities).FirstOrDefaultAsync(x => x.id == mobilAku.RegionsId);

            if (!TryValidateModel(mobilAku))
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));

            var result = _context.MobilAku.Add(mobilAku);
            await _context.SaveChangesAsync();

            MobilAkuVM vm = new MobilAkuVM();
            PopulateVM(vm, mobilAku);

            return Ok(vm);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMobilAku(int key, string values)
        {
            var mobilAku = await _context.MobilAku.FirstOrDefaultAsync(o => o.id == key);

            var convertedValues = JsonConvert.DeserializeObject<IDictionary>(values);

            PopulateModel(mobilAku, convertedValues);

            //TODO: DevExpress componenti TryValidate'yi forceladığı için relational eklemeleri şimdilik hızlıca bu şekilde tek tek ve include ile yaptım. ilerde düzeltilecek. Gerekirse tryValidate kullanmayız.    
            if (convertedValues.Contains("citiesId"))
                mobilAku.Cities = await _context.Cities.Include(x => x.Regions).FirstOrDefaultAsync(x => x.id == Convert.ToInt32(convertedValues["citiesId"]));
            else
                mobilAku.Cities = await _context.Cities.Include(x => x.Regions).FirstOrDefaultAsync(x => x.id == mobilAku.CitiesId);
            if (convertedValues.Contains("regionsId"))
                mobilAku.Regions = await _context.Regions.Include(x => x.Cities).FirstOrDefaultAsync(x => x.id == Convert.ToInt32(convertedValues["regionsId"]));
            else
                mobilAku.Regions = await _context.Regions.Include(x => x.Cities).FirstOrDefaultAsync(x => x.id == mobilAku.RegionsId);

            if (!TryValidateModel(mobilAku))
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));

            await _context.SaveChangesAsync();

            //TODO:Relational datayı test için bu şekilde ekstra sorguyla doldurdum. Ileride mevcut aku id'lerden daha hızlı 2 küçük city ve region sorgusuyla yapmayı test et.
            // var mobilAkuWithCitiesRegions = await _context.MobilAku.Include(x => x.Cities).Include(x => x.Regions).FirstOrDefaultAsync(x => x.id == mobilAku.id);

            MobilAkuVM vm = new MobilAkuVM();
            PopulateVM(vm, mobilAku);
            return Ok(vm);
        }

        [HttpDelete]
        public async Task DeleteMobilAku(int key)
        {
            var mobilAku = await _context.MobilAku.FirstAsync(o => o.id == key);
            _context.MobilAku.Remove(mobilAku);
            await _context.SaveChangesAsync();
        }


        //*Additional endpoints
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var mobilAkus = await _context.MobilAku.Include(x => x.Cities).Include(x => x.Regions).ToListAsync();

                List<MobilAkuVM> vmList = new();

                foreach (var aku in mobilAkus)
                {
                    MobilAkuVM vm = new MobilAkuVM();
                    PopulateVM(vm, aku);
                }

                return Ok(vmList);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //! Detay bilgisi almak için kullanıyorum. Testte tek bir ViewModel kullandığımdan (test olduğu için) tüm propertyler gidiyor, ileride sadece detayVM ile gerekliler yollanacak.
        //! şimdilik hepsini yolluyorum.
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var mobilAku = await _context.MobilAku.Include(x => x.Cities).Include(x => x.Regions).FirstOrDefaultAsync(x => x.id == id);

                MobilAkuVM vm = new();
                PopulateVM(vm, mobilAku);

                return Ok(vm);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Markers()
        {
            var data = await _context.Cities.Select(x => new MarkerVM()
            {
                location = x.latitude + ", " + x.longitude,
                tooltip = new Tooltip()
                {
                    text = x.name,
                }
            })
            .ToListAsync();

            return Ok(data);
        }

        //TODO:Dökümantasyona göre (kendi api'si string olarak sadece gerekli datayı yolluyor, tam bir nesne yollamıyor automapper'da ekstra config) (bir sıkıntı çıkmasın düzgün çalıştığını göreyim) şimdilik bu şekilde, ileride direkt AutoMapper ile.
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
            if (values.Contains("citiesId"))
                mobilAku.CitiesId = Convert.ToInt32(values["citiesId"]);
            if (values.Contains("regionsId"))
                mobilAku.RegionsId = Convert.ToInt32(values["regionsId"]);
        }

        private void PopulateVM(MobilAkuVM mobilAkuVM, MobilAku akuNesne)
        {
            mobilAkuVM.id = akuNesne.id;
            mobilAkuVM.tenant_id = akuNesne.tenant_id;
            mobilAkuVM.report_file_process_id = akuNesne.report_file_process_id;
            mobilAkuVM.location = akuNesne.location;
            mobilAkuVM.asset_num = akuNesne.asset_num;
            mobilAkuVM.n_of_ac = akuNesne.n_of_ac;
            mobilAkuVM.n_of_ne = akuNesne.n_of_ne;
            mobilAkuVM.battery_age = akuNesne.battery_age;
            mobilAkuVM.n_of_partial_charge = akuNesne.n_of_partial_charge;
            mobilAkuVM.n_of_generator = akuNesne.n_of_generator;
            mobilAkuVM.n_of_air_con = akuNesne.n_of_air_con;
            mobilAkuVM.max_ac_duration = akuNesne.max_ac_duration;
            mobilAkuVM.mx_afad = akuNesne.mx_afad;
            mobilAkuVM.totalpower_loc = akuNesne.totalpower_loc;
            mobilAkuVM.ideal_working_hour = akuNesne.ideal_working_hour;
            mobilAkuVM.back_sites = akuNesne.back_sites;
            mobilAkuVM.technology = akuNesne.technology;
            mobilAkuVM.remaining_battery_lifetime = akuNesne.remaining_battery_lifetime;
            mobilAkuVM.remaining_battery_lifetime_cast_int = akuNesne.remaining_battery_lifetime_cast_int;
            mobilAkuVM.recommendation = akuNesne.recommendation;
            mobilAkuVM.additional_info = akuNesne.additional_info;
            mobilAkuVM.current_date = akuNesne.current_date;
            mobilAkuVM.RegionsId = akuNesne.RegionsId;
            mobilAkuVM.CitiesId = akuNesne.CitiesId;
            mobilAkuVM.RegionsName = akuNesne.Regions.name;
            mobilAkuVM.CitiesName = akuNesne.Cities.name;
        }


    }
}