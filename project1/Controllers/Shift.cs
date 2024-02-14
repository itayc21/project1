using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using project1.Models;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace project1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ShiftController : ApiController
    {
        private static project1BL bl = new project1BL();

        // GET: api/Shift
        public IEnumerable<Shift> Get()
        {
            return bl.GetAllShifts();
        }

        // GET: api/Shift/5
        public Shift Get(int id)
        {
            return bl.getshift(id);
        }

        // POST: api/Shift
        public string Post(Shift shift)
        {
           return bl.AddShift(shift); // Removed unnecessary type specification
            
        }

        // PUT: api/Shift/5
        public void Put(int id, [FromBody] string value)
        {
            // Implementation for updating a shift (not currently implemented in your code)
        }

        // DELETE: api/Shift/5
        public string Delete(int id)
        {
            return bl.RemoveShift(id);

        }
    }
}
