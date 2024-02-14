using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using project1.Models;

namespace project1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmployeeController : ApiController
    {
        private static project1BL bl = new project1BL();

        // GET: api/Employee
        public IEnumerable<Employee> Get()
        {
            return bl.GetAllEmployees();
        }

        // GET: api/Employee/5
        public Employee Get(int id)
        {
            return bl.GetEmployee(id);
        }

        // POST: api/Employee
        public string Post(Employee emp)
        {
            bl.AddEmployee(emp);
            return "Added";
        }

        // PUT: api/Employee/5
        public string Put(int id, Employee emp)
        {
            bl.UpdateEmployee(id, emp);
            return "Updated";
        }

        // DELETE: api/Employee/5
        public string Delete(int id)
        {
            bl.DeleteEmployee(id);
            return "Deleted";
        }
    }
}
