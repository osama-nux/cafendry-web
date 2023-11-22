using AutoMapper;
using CyfendryAcademyWebApp.Data;
using CyfendryAcademyWebApp.Models.RequestModels;
using CyfendryAcademyWebApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyfendryAcademyWebApp.MapperProfile
{
    public class MapperConfig
    {
        public static Mapper Init()
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<RegisterRequestModel, User>();
                cfg.CreateMap<CoursePlan, CoursePlanListVM>();
                cfg.CreateMap<Cours, CourseListVM>().ForMember(dest => dest.CoursePlans, opt => opt.MapFrom(src => src.CoursePlans));
                cfg.CreateMap<UserCours, UserCourseVM>();
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}