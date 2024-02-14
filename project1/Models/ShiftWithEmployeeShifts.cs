using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project1.Models
{
    public class ShiftWithEmployeeShifts
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<EmployeeShift> EmployeeShifts { get; set; }
        
    }
    public class EmployeeShiftViewModel
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
    }
}