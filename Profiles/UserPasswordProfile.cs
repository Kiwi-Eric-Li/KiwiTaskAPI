using AutoMapper;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Profiles
{
    public class UserPasswordProfile: Profile
    {
        public UserPasswordProfile()
        {
            CreateMap<UserPassword, RegisterDto>();
        }
    }
}
