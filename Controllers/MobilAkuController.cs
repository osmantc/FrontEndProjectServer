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


using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Server.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]/[action]")]
    public class MobilAkuController : Microsoft.AspNetCore.Mvc.Controller
    {
        private MyDbContext _context { get; set; }

        public MobilAkuController(MyDbContext context)
        {
            _context = context;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public object MobilAkus(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_context.MobilAku, loadOptions);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult InsertMobilAku(string values)
        {
            var mobilAku = new MobilAku();
            JsonConvert.PopulateObject(values, mobilAku);

            if (!TryValidateModel(mobilAku))
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));

            _context.MobilAku.Add(mobilAku);
            _context.SaveChanges();

            return Ok(mobilAku);
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        public IActionResult UpdateMobilAku(int key, string values)
        {
            var mobilAku = _context.MobilAku.First(o => o.id == key);
            JsonConvert.PopulateObject(values, mobilAku);

            if (!TryValidateModel(mobilAku))
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));

            _context.SaveChanges();

            return Ok(mobilAku);
        }

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        public void DeleteMobilAku(int key)
        {
            var mobilAku = _context.MobilAku.First(o => o.id == key);
            _context.MobilAku.Remove(mobilAku);
            _context.SaveChanges();
        }



        //*Additional endpoints
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var mobilAkus = _context.MobilAku.ToList();

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


    }
}