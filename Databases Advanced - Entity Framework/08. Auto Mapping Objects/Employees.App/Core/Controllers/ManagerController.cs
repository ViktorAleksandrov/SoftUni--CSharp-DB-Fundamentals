namespace Employees.App.Core.Controllers
{
    using System;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Contracts;
    using Data;
    using Dtos;
    using Models;

    public class ManagerController : IManagerController
    {
        private readonly EmployeesDbContext context;
        private readonly IMapper mapper;

        public ManagerController(EmployeesDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void SetManager(int employeeId, int managerId)
        {
            Employee employee = this.context.Employees.Find(employeeId);
            Employee manager = this.context.Employees.Find(managerId);

            if (employee == null || manager == null)
            {
                throw new ArgumentException("Invalid ID");
            }

            employee.Manager = manager;

            this.context.SaveChanges();
        }

        public ManagerDto GetManagerInfo(int employeeId)
        {
            ManagerDto managerDto = this.context.Employees
                .Where(e => e.Id == employeeId)
                .ProjectTo<ManagerDto>()
                .SingleOrDefault();

            if (managerDto == null)
            {
                throw new ArgumentException("Invalid ID");
            }

            return managerDto;
        }
    }
}
