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
    public class EmployeeshiftCon : ApiController

    {
        private static project1BL bl = new project1BL();

        // GET: api/Employeeshiftcon
        public IEnumerable<EmployeeShift> Get()
        {
            return bl.GetAllEmployeeShifts();
        }

        // GET: api/Employeeshiftcon/5
        public EmployeeShift Get(int id)
        {
            return bl.GetEmployeeShift(id);
        }

        // POST: api/Employeeshiftcon
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Employeeshiftcon/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Employeeshiftcon/5
        public void Delete(int id)
        {
        }
    }
}
