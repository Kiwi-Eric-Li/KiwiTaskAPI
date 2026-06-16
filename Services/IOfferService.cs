using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Services
{
    public interface IOfferService
    {
        Task<IEnumerable<TaskOffers>> GetTaskOffersAsync(Guid taskid);
        Task<IEnumerable<TaskOffers>> GetTaskOffersByTaskIdAsync(Guid taskid);
        Task<int> CreateOfferAsync(Guid taskid, Guid userid, OfferCreateDto dto);
        Task<int> AcceptOfferAsync(Guid taskid, Guid tasker_id, int offerid);
        Task<int> CancelOfferAsync(Guid taskid, int id);
        Task<int> DeclineInvitationAsync(Guid taskid);
        Task<int> ConfirmInvitationAsync(Guid taskid);
    }
}
