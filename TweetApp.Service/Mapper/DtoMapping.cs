using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetApp.Domain.Models;
using TweetApp.Service.ModelDto;

namespace TweetApp.Service.Mapper
{
    public class DtoMapping:Profile
    {
        public DtoMapping()
        {
            CreateMap<User,RegisterModelDto>().ReverseMap();
            CreateMap<Tweet, TweetDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<LoginModel, LoginModelDto>().ReverseMap();
            CreateMap<ResetPasswordModel, ResetPasswordModelDto>().ReverseMap();
            CreateMap<ForgetPasswordModel, ForgetPasswordModelDto>().ReverseMap();
        }
    }
}
