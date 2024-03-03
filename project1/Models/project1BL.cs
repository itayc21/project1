using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace project1.Models
{
    public class project1BL


    {
        ProjectDBEntities Db = new ProjectDBEntities();
        public project1BL()
        {
            
            Db.Configuration.LazyLoadingEnabled = false;
            Db.Configuration.ProxyCreationEnabled = false;

        }


        public List<Employee> GetAllEmployees()
        {

            return Db.Employees.ToList();
        }
        public Employee GetEmployee(int id)
        {
            return Db.Employees.Where(x => x.ID == id).First();
        }
        public void AddEmployee(Employee emp)
        {
            Db.Employees.Add(emp);
            Db.SaveChanges();



        }

        public void UpdateEmployee(int id, Employee emp)
        {
            var currentEmp = Db.Employees.Where(x => x.ID == id).First();
            currentEmp.First_Name = emp.First_Name;
            currentEmp.Last_Name = emp.Last_Name;
            currentEmp.Start_Work_Year = emp.Start_Work_Year;
            currentEmp.DepartmentID = emp.DepartmentID;

            Db.SaveChanges();


        }
        public void DeleteEmployee(int employeeId)
        {
            try
            {
                using (var dbContext = new ProjectDBEntities())
                {
                    var employee = dbContext.Employees.Find(employeeId);
                    if (employee == null)
                    {
                        throw new Exception("Employee not found");
                    }

                 
                    var employeeShifts = dbContext.EmployeeShifts.Where(es => es.EmployeeID == employeeId);

                   
                    foreach (var employeeShift in employeeShifts)
                    {
                        dbContext.EmployeeShifts.Remove(employeeShift);
                    }

                    var affectedDepartments = dbContext.Departments.Where(d => d.Manager == employeeId);

                    
                    foreach (var department in affectedDepartments)
                    {
                        department.Manager = null;
                    }

                    
                    dbContext.Employees.Remove(employee);

                    
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error deleting employee with ID {employeeId}: {ex.Message}");
                throw; 
            }
        }


        public List<Department> GetALLDepartments()
        {
            return Db.Departments.ToList();

        }

        public Department GetDepartment(int id)
        {
            return Db.Departments.Where(x => x.ID == id).First();
        }


        public void AddDepartment(Department dep)
        {
            try
            {
                Db.Departments.Add(dep);
                Db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("DbUpdateException: " + ex.Message);
                Console.WriteLine("StackTrace: " + ex.StackTrace);


                throw new Exception("Failed to add department. Please check the server logs for more details.");
            }
        }
        public void UpdateDepartment(int id, Department dep)
        {
            try
            {
                var currentDep = Db.Departments.FirstOrDefault(x => x.ID == id);

                if (currentDep != null)
                {
                    
                    currentDep.Name = dep.Name;

                   
                    if (dep.Manager != 0) 
                    {
                        currentDep.Manager = dep.Manager;
                    }

                    // Save changes
                    Db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
             
                Console.WriteLine($"Error updating department with ID {id}: {ex.Message}");
                throw;
            }
        }

        public string DeleteDepartment(int id)
        {
            var currentdep = Db.Departments.Where(x => x.ID == id).First();
            Db.Departments.Remove(currentdep);
            Db.SaveChanges();
            return "Deleted";


        }
        public List<Shift> GetAllShifts()
        {

            return Db.Shifts.ToList();
        }
        public Shift getshift(int id)
        {
            return Db.Shifts.FirstOrDefault(x => x.ID == id);
        }
        public int GetNextAvailableId()
        {
            return Db.Shifts.Max(s => s.ID) + 1;
        }
        public string AddShift(Shift shift)
        {
            try
            {
                // Add the shift to the database
                shift.ID = GetNextAvailableId();
                Db.Shifts.Add(shift);
                Db.SaveChanges();


                Db.SaveChanges();

                return "Added";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }


        public string RemoveShift(int id)
        {
            Shift p = Db.Shifts.Where(x => x.ID == id).First();
            if (p != null)
            {
                Db.Shifts.Remove(p);
                Db.SaveChanges();
            }

            return "Deleted";

        }
        public List<EmployeeShift> GetAllEmployeeShifts()
        {
            return Db.EmployeeShifts.ToList();
        }
        public EmployeeShift GetEmployeeShift(int id)
        {
            return Db.EmployeeShifts.FirstOrDefault(x => x.ID == id);
        }
        public string removeEmployeeshift(int id)
        {
            EmployeeShift p = Db.EmployeeShifts.Where(x => x.ID == id).First();
            if (p != null)
            {
                Db.EmployeeShifts.Remove(p);
                Db.SaveChanges();
            }

            return "Deleted";

        }
        public bool Login(string Username, string Password)
        {
            var currentUser = Db.Users.FirstOrDefault(x => x.User_Name == Username);

            if (currentUser != null)
            {
                bool passwordsMatch = Password == currentUser.Password;
                return passwordsMatch;
            }
            else
            {
                return false;
            }
        }
        public List<ShiftWithEmployeeShifts> GetAllShiftsWithEmployeeShifts()
        {
            var query = from shift in Db.Shifts
                        join employeeShift in Db.EmployeeShifts
                        on shift.ID equals employeeShift.ShiftID into employeeShifts
                        select new ShiftWithEmployeeShifts
                        {
                            ID = shift.ID,
                            Date = shift.Date.HasValue ? shift.Date.Value : DateTime.MinValue,
                            StartTime = shift.Start_Time.HasValue ? shift.Start_Time.Value : TimeSpan.Zero,
                            EndTime = shift.End_Time.HasValue ? shift.End_Time.Value : TimeSpan.Zero,
                            EmployeeShifts = employeeShifts.ToList()
                        };

            return query.ToList();
        }
        public ShiftWithEmployeeShifts GetShiftWithEmployeeShifts(int id)
        {
            var query = from shift in Db.Shifts
                        where shift.ID == id
                        join employeeShift in Db.EmployeeShifts
                        on shift.ID equals employeeShift.ShiftID into employeeShifts
                        select new ShiftWithEmployeeShifts
                        {
                            ID = shift.ID,
                            Date = shift.Date ?? DateTime.MinValue,
                            StartTime = shift.Start_Time ?? TimeSpan.Zero,
                            EndTime = shift.End_Time ?? TimeSpan.Zero,
                            EmployeeShifts = employeeShifts.ToList()
                        };

            return query.FirstOrDefault();
        }
        public string AddShiftWithEmployeeShifts(ShiftWithEmployeeShifts shiftWithEmployeeShifts)
        {
            try
            {
                using (var context = new ProjectDBEntities()) 
                {
                  
                    var newShift = new Shift
                    {
                        Date = shiftWithEmployeeShifts.Date,
                        Start_Time = shiftWithEmployeeShifts.StartTime,
                        End_Time = shiftWithEmployeeShifts.EndTime
                        
                    };

                    context.Shifts.Add(newShift);
                    context.SaveChanges();

                  
                    int newShiftId = newShift.ID;

                  
                    foreach (int employeeId in shiftWithEmployeeShifts.EmployeeShifts.Select(e => e.EmployeeID))
                    {
                        
                        var newEmployeeShift = new EmployeeShift
                        {
                            ShiftID = newShiftId,
                            EmployeeID = employeeId,
                           
                        };

                      
                        context.EmployeeShifts.Add(newEmployeeShift);
                    }

                    
                    context.SaveChanges();
                }

                return "Added";
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error adding shift with employee shifts: {ex.Message}");
                return "Failed to add shift with employee shifts. Please check the server logs for more details.";
            }
        }

        public List<User> getallusers()
        {

            return Db.Users.ToList();
        }
    }
}