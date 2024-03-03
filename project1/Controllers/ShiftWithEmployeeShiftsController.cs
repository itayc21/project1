using project1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace project1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ShiftWithEmployeeShiftsController : ApiController
    {
        private static project1BL bl = new project1BL();
        // GET: api/ShiftWithEmployeeShifts
        public IEnumerable<ShiftWithEmployeeShifts> Get()
        {
            return bl.GetAllShiftsWithEmployeeShifts();
        }

        // GET: api/ShiftWithEmployeeShifts/5
        public ShiftWithEmployeeShifts Get(int id)
        {
            return bl.GetShiftWithEmployeeShifts(id);
        }

        // POST: api/ShiftWithEmployeeShifts
        public string Post([FromBody] ShiftWithEmployeeShifts shiftWithEmployeeShifts)
        {
           
            
           bl.AddShiftWithEmployeeShifts(shiftWithEmployeeShifts);
            return "Added";
         
        }

        // PUT: api/ShiftWithEmployeeShifts/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ShiftWithEmployeeShifts/5
        public void Delete(int id)
        {
        }
    }
}
