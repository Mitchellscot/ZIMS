using AutoMapper;
using ZIMS.Data.Entities;
using ZIMS.Models.Users;

//this class doesn't do shit as automapper can't find it
namespace ZIMS.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, AuthenticateResponse>();
        }
    }
}