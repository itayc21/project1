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
    public class DepartmentController : ApiController
    {
        private static project1BL bl = new project1BL();

        // GET: api/Department
        public IEnumerable<Department> Get()  
        {
            return bl.GetALLDepartments();
        }

        // GET: api/Department/5
        public Department Get(int id)  
        {
            return bl.GetDepartment(id);
        }

        // POST: api/Department
        public string Post(Department dep)  
        {
            bl.AddDepartment(dep);
            return "Added";
        }

        // PUT: api/Department/5
        public string Put(int id, Department dep)  
        {
            bl.UpdateDepartment(id, dep);
            return "Updated";
        }

        // DELETE: api/Department/5
        public string Delete(int id)
        {
            bl.DeleteDepartment(id);
            return "Deleted";
        }
    }
}
