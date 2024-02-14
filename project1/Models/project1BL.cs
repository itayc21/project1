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
            // Disable lazy loading for the DbContext
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
        public void DeleteEmployee(int id)
        {
            var currentEmp = Db.Employees.Where(x => x.ID == id).First();
            Db.Employees.Remove(currentEmp);
            Db.SaveChanges();


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

                // Log the details or handle the exception as needed
                // You may want to use a proper logging framework (e.g., Serilog, NLog) to log the exception to a file or database

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
                    // Update department details
                    currentDep.Name = dep.Name;

                    // Only update the manager if it's included in the update data
                    if (dep.Manager != 0) // Assuming 0 is not a valid manager ID
                    {
                        currentDep.Manager = dep.Manager;
                    }

                    // Save changes
                    Db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
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
            shift.ID = GetNextAvailableId();
            Db.Shifts.Add(shift);
            Db.SaveChanges();
            return "Added";
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
            return Db.EmployeeShifts.FirstOrDefault(x=>x.ID==id);
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
                // Create a new Shift entity
                var newShift = new Shift
                {
                    Date = shiftWithEmployeeShifts.Date,
                    Start_Time = shiftWithEmployeeShifts.StartTime,
                    End_Time = shiftWithEmployeeShifts.EndTime
                    // Add other properties as needed
                };

                // Add the new shift to the Shifts table
                Db.Shifts.Add(newShift);
                Db.SaveChanges();

                // Get the ID of the newly added shift
                int newShiftId = newShift.ID;

                // Add employee shifts associated with the new shift
                foreach (var employeeShiftViewModel in shiftWithEmployeeShifts.EmployeeShifts)
                {
                    var newEmployeeShift = new EmployeeShift
                    {
                        ShiftID = newShiftId,
                        EmployeeID = employeeShiftViewModel.EmployeeID
                        // Add other properties as needed
                    };

                    Db.EmployeeShifts.Add(newEmployeeShift);
                }

                Db.SaveChanges();

                return "Added";
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error adding shift with employee shifts: {ex.Message}");
                return "Failed to add shift with employee shifts. Please check the server logs for more details.";
            }
        }



    }
}