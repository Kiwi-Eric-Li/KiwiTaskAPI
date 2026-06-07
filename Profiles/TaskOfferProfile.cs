using AutoMapper;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Profiles
{
    public class TaskOfferProfile: Profile
    {
        public TaskOfferProfile()
        {
            CreateMap<OfferCreateDto, TaskOffers>();
            CreateMap<TaskOffers, TaskOffersDto>().ForMember(
                dest => dest.is_expired,
                opt => opt.MapFrom(src => src.expired_at <= DateTime.UtcNow)
            );
        }
    }
}
