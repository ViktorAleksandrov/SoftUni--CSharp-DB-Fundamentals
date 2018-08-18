namespace Employees.App.Core
{
    using AutoMapper;

    using Dtos;
    using Models;

    public class EmployeeDbProfile : Profile
    {
        public EmployeeDbProfile()
        {
            this.CreateMap<Employee, EmployeeDto>().ReverseMap();

            this.CreateMap<Employee, EmployeePersonalInfoDto>().ReverseMap();

            this.CreateMap<Employee, EmployeesOlderThanDto>().ReverseMap();

            this.CreateMap<Employee, ManagerDto>()
                .ForMember(dest => dest.EmployeeDtos, from => from.MapFrom(m => m.ManagerEmployees))
                .ReverseMap();
        }
    }
}
