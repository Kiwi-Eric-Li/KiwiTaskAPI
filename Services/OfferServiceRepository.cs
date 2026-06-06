using AutoMapper;
using KiwiTaskAPI.Database;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace KiwiTaskAPI.Services
{
    public class OfferServiceRepository : IOfferService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OfferServiceRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IEnumerable<TaskOffers>> GetTaskOffersAsync(Guid taskid)
        {
            return await _context.task_offers.Where(t => t.task_id == taskid).ToListAsync();
        }


        public async Task<int> CreateOfferAsync(Guid taskid, Guid userid, OfferCreateDto dto)
        {
            var taskOffer = _mapper.Map<TaskOffers>(dto);

            if(dto.attachments.Count() > 0)
            {
                var attachments = new List<object>();
                foreach (var at in dto.attachments)
                {
                    attachments.Add(new
                    {
                        url = at,
                        type = "image"
                    });
                }
                taskOffer.attachments = JsonSerializer.Serialize(attachments);
            }
            else
            {
                taskOffer.attachments = "[]";
            }

            taskOffer.task_id = taskid;
            taskOffer.user_id = userid;
            taskOffer.created_at = DateTime.Now;
            taskOffer.expired_at = DateTime.Now.AddMonths(1);
            await _context.task_offers.AddAsync(taskOffer);
            return await _context.SaveChangesAsync();
        }
    }
}
