using AutoMapper;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.DataAccess.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.DataAccess.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Your CreateMap and other mapping configurations here
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));



        }

    }
}
