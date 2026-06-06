using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Services
{
    public interface IOfferService
    {
        Task<IEnumerable<TaskOffers>> GetTaskOffersAsync(Guid taskid);
        Task<int> CreateOfferAsync(Guid taskid, Guid userid, OfferCreateDto dto);
    }
}
