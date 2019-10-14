using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IOT_Fire_Detection.Services;
namespace IOT_Fire_Detection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class dataController : ControllerBase
    {
        private readonly DataService dataService;

        public dataController(){
            dataService = new DataService();
        }

        // GET: api/data
        [HttpGet]
        public async Task<object> GetAsync(){
            return await dataService.getLatest();
        }

        // GET: api/data/lastminute
        [HttpGet]
        [Route("lastminute")]
        public async Task<IEnumerable<object>> GetLastMinute(){
            return await dataService.getLastMinute();
        }

        // GET: api/data/lasthour
        [HttpGet]
        [Route("lasthour")]
        public async Task<IEnumerable<object>> GetLastHour()
        {
            return await dataService.getLastHour();
        }

        // GET: api/data/lastday
        [HttpGet]
        [Route("lastday")]
        public async Task<IEnumerable<object>> GetLastDay()
        {
            return await dataService.getLastDay();
        }

        // GET: api/data/lastweek
        [HttpGet]
        [Route("lastweek")]
        public async Task<IEnumerable<object>> GetLastWeek()
        {
            return await dataService.getLastWeek();
        }
    }
}
