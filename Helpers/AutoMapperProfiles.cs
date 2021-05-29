using AutoMapper;
using ZIMS.Data.Entities;
using ZIMS.Models.Users;

namespace ZIMS.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            
            this.CreateMap<User, AuthenticateRequest>();
        }
    }
}