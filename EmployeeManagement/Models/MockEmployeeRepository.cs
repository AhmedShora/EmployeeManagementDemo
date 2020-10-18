using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employees;
        public MockEmployeeRepository()
        {
            _employees = new List<Employee>() {
                new Employee(){Id=1,Name="ahmed" ,Email="ahmed1@aa.com",Department=Dept.HR },
                new Employee(){Id=2,Name="mohamed" ,Email="mohamed2@aa.com",Department=Dept.IT },
                new Employee(){Id=3,Name="samy" ,Email="samy3@aa.com",Department=Dept.Sales }
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employees.Max(aa => aa.Id) + 1;
            _employees.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            var emp = _employees.FirstOrDefault(aa=>aa.Id==id);
            if (emp!=null)
            {
                _employees.Remove(emp);
            }
            return emp;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employees;
        }

        public Employee GetEmployee(int Id)
        {
            return _employees.FirstOrDefault(e => e.Id == Id);
        }

        public Employee Update(Employee changesEmployee)
        {
            var emp = _employees.FirstOrDefault(aa => aa.Id == changesEmployee.Id);
            if (emp != null)
            {
                emp.Name = changesEmployee.Name;
                emp.Email = changesEmployee.Email;
                emp.Department = changesEmployee.Department;
            }
            return emp;
        }
    }
}
