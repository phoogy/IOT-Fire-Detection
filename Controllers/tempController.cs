using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IOT_Fire_Detection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempController : ControllerBase
    {
        // GET: api/temp
        [HttpGet]
        public object Get()
        {
            var r = new Random();
            var temp = r.Next(20, 25);
            var now = DateTime.Now;
            var data = new { timestamp = now, temp = temp };
            return data;
        }

        // GET: api/temp/lastminute
        [HttpGet]
        [Route("lastminute")]
        public IEnumerable<object> GetLastMinute()
        {
            var list = new List<object>();

            // test data
            var now = DateTime.Now;
            var r = new Random();
            for (int i=0; i< 60; i++)
            {
                var temp = r.Next(20, 25);
                var data = new { timestamp = now.AddMinutes(-1).AddSeconds(i), temp = temp };
                list.Add(data);
            }
            return list;
        }

        // GET: api/temp/lasthour
        [HttpGet]
        [Route("lasthour")]
        public IEnumerable<object> GetLastHour()
        {
            var list = new List<object>();

            // test data
            var now = DateTime.Now;
            var r = new Random();
            for (int i = 0; i < 60; i++)
            {
                var temp = r.Next(20, 25);
                var data = new { timestamp = now.AddHours(-1).AddMinutes(i), temp = temp };
                list.Add(data);
            }
            return list;
        }

        // GET: api/temp/lastday
        [HttpGet]
        [Route("lastday")]
        public IEnumerable<object> GetLastDay()
        {
            var list = new List<object>();

            // test data
            var now = DateTime.Now;
            var r = new Random();
            for (int i = 0; i < 24; i++)
            {
                var temp = r.Next(20, 25);
                var data = new { timestamp = now.AddDays(-1).AddHours(i), temp = temp };
                list.Add(data);
            }
            return list;
        }
    }
}
