using System;

namespace Employee.App.Common.Models
{
    public class EmployeeContacts
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
