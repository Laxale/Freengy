using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Freengy.WebApi.Controllers
{
//    [Authorize]
    public class ValuesController : ApiController
    {
        // this has to be static - controller is created for each request
        private static readonly List<string> values = new List<string>
        {
            "value0", "value2", "value3"
        };

        // GET api/values
        public IEnumerable<string> Get()
        {
            return values;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return values.ElementAt(id);
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            values.Add(value);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
            values[id] = value;
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            values.RemoveAt(id);
        }
    }
}
