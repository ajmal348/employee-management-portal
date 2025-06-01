using AutoMapper;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserAccount!.Username))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.UserAccount!.Role))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.ToString()));

            CreateMap<CreateEmployeeDto, Employee>()
               .ForMember(dest => dest.Department, opt => opt.MapFrom(src => Enum.Parse<Department>(src.Department)))
               .ForMember(dest => dest.UserAccount, opt => opt.Ignore()); 

        }
    }
}
