namespace Employees.App.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Contracts;
    using Data;
    using Dtos;
    using Models;

    public class EmployeeController : IEmployeeController
    {
        private readonly EmployeesDbContext context;
        private readonly IMapper mapper;

        public EmployeeController(EmployeesDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void AddEmployee(EmployeeDto employeeDto)
        {
            Employee employee = this.mapper.Map<Employee>(employeeDto);

            this.context.Employees.Add(employee);

            this.context.SaveChanges();
        }

        public void SetBirthday(int employeeId, DateTime date)
        {
            Employee employee = this.context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid ID");
            }

            employee.Bitrhday = date;

            this.context.SaveChanges();
        }

        public void SetAddress(int employeeId, string address)
        {
            Employee employee = this.context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid ID");
            }

            employee.Address = address;

            this.context.SaveChanges();
        }

        public EmployeeDto GetEmployeeInfo(int employeeId)
        {
            Employee employee = this.context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid ID");
            }

            EmployeeDto employeeDto = this.mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public EmployeePersonalInfoDto GetEmployeePersonalInfo(int employeeId)
        {
            Employee employee = this.context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid ID");
            }

            EmployeePersonalInfoDto employeeDto = this.mapper.Map<EmployeePersonalInfoDto>(employee);

            return employeeDto;
        }

        public IEnumerable<EmployeesOlderThanDto> ListEmployeesOlderThan(int age)
        {
            EmployeesOlderThanDto[] employees = this.context.Employees
                .Where(e => DateTime.Now.Year - e.Bitrhday.Value.Year > age)
                .ProjectTo<EmployeesOlderThanDto>()
                .ToArray();

            return employees;
        }
    }
}
