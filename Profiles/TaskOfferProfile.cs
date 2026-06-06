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
        }
    }
}
