using AutoMapper;
using EmployeesCrud.Models;
using EmployeesCrud.ViewModels;

namespace EmployeesCrud.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EmployeeMaster, EmployeesViewModel>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.CountryName))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.StateName))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.CityName))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Row_Id, opt => opt.MapFrom(src => src.Row_Id));
        }
    }
}
