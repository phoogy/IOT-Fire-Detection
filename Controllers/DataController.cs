using IOT_Fire_Detection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IOT_Fire_Detection.Controllers
{
    public class DataController : ApiController
    {
        private readonly DataService dataService;

        public DataController()
        {
            dataService = new DataService();
        }

        // GET: api/data
        [HttpGet]
        public async Task<object> GetAsync()
        {
            return await dataService.getLatest();
        }

        // GET: api/data/lastminute
        [HttpGet]
        [Route("lastminute")]
        public async Task<IEnumerable<object>> GetLastMinute()
        {
            return await dataService.get60Seconds(DateTime.Now.AddSeconds(-60));
        }

        // GET: api/data/lasthour
        [HttpGet]
        [Route("lasthour")]
        public async Task<IEnumerable<object>> GetLastHour()
        {
            return await dataService.get60Minutes(DateTime.Now.AddMinutes(-60));
        }

        // GET: api/data/lastday
        [HttpGet]
        [Route("lastday")]
        public async Task<IEnumerable<object>> GetLastDay()
        {
            return await dataService.get24Hours(DateTime.Now.AddHours(-24));
        }

        // GET: api/data/lastweek
        [HttpGet]
        [Route("lastweek")]
        public async Task<IEnumerable<object>> GetLastWeek()
        {
            return await dataService.get7Days(DateTime.Now.AddDays(-7));
        }

        // GET: api/data/lastmonth
        [HttpGet]
        [Route("lastmonth")]
        public async Task<IEnumerable<object>> GetLastMonth()
        {
            return await dataService.get30Days(DateTime.Now.AddDays(-30));
        }
    }
}
