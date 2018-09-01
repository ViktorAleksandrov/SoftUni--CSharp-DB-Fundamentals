using AutoMapper;
using CarDealer.App.Dto.Import;
using CarDealer.Models;

namespace CarDealer.App
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SupplierImportDto, Supplier>();
            this.CreateMap<PartDto, Part>();
            this.CreateMap<CarDto, Car>();
            this.CreateMap<CustomerDto, Customer>();
        }
    }
}
