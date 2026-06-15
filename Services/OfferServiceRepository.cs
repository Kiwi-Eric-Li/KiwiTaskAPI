using AutoMapper;
using KiwiTaskAPI.Database;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Hubs;
using KiwiTaskAPI.Models;
using KiwiTaskAPI.Types;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;

namespace KiwiTaskAPI.Services
{
    public class OfferServiceRepository : IOfferService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<TaskNotificationsHub> _hub;
        private readonly ILogger<IOfferService> _log;

        public OfferServiceRepository(AppDbContext context, IMapper mapper, IHubContext<TaskNotificationsHub> hub, ILogger<IOfferService> log)
        {
            _context = context;
            _mapper = mapper;
            _hub = hub;
            _log = log;
        }

        public async Task<int> DeclineInvitationAsync(Guid taskid)
        {
            var match = await _context.task_matches.FirstOrDefaultAsync(m => m.task_id == taskid);

            if (match == null)
            {
                throw new Exception("Match record not found.");
            }

            _context.task_matches.Remove(match);

            var task = await _context.tasks.FirstOrDefaultAsync(t => t.id == taskid);

            if (task != null)
            {
                task.status = "Open";
            }

            return await _context.SaveChangesAsync();
        }


        public async Task<int> CancelOfferAsync(Guid taskid, int offerid)
        {
            var offer = await _context.task_offers.FirstOrDefaultAsync(o => o.id == offerid);

            if (offer == null)
            {
                throw new Exception("Offer not found.");
            }

            var match = await _context.task_matches.FirstOrDefaultAsync(m => m.task_id == offer.task_id && m.tasker_id == offer.user_id);

            if (match == null)
            {
                throw new Exception("Match record not found.");
            }

            _context.task_matches.Remove(match);

            var task = await _context.tasks.FirstOrDefaultAsync(t => t.id == offer.task_id);

            if (task != null)
            {
                task.status = "Open";
            }

            // broadcast to viewers of this task page
            await _hub.Clients.Group(HubGroups.Task(taskid)).SendAsync(HubEvents.TaskOfferCancelled, task.id);
            _log.LogInformation("Broadcase Event = {Event}, Group = {Group}", HubEvents.TaskOfferCancelled, HubGroups.Task(taskid));

            return await _context.SaveChangesAsync();
        }


        public async Task<int> AcceptOfferAsync(Guid taskid, Guid tasker_id, int offerid)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingMatches = await _context.task_matches.Where(x => x.task_id == taskid).ToListAsync();

                if (existingMatches.Any())
                {
                    _context.task_matches.RemoveRange(existingMatches);
                    await _context.SaveChangesAsync();
                }

                // insert matched record
                var taskMatch = new TaskMatches
                {
                    task_id = taskid,
                    tasker_id = tasker_id,
                    matched_at = DateTime.UtcNow,
                    confirmed = 0,
                    confirm_expires = DateTime.UtcNow.AddDays(1)
                };
                await _context.task_matches.AddAsync(taskMatch);
                await _context.SaveChangesAsync();

                // 2. update task status
                var task = await _context.tasks.FirstOrDefaultAsync(t => t.id == taskid);
                if (task == null)
                    throw new Exception("Task not found");

                task.status = "Matched";

                // 3. save (part of transaction)
                var result = await _context.SaveChangesAsync();

                // 4. commit transaction
                await transaction.CommitAsync();

                // 5. after commit -> broadcast event
                var acceptedDto = new OfferAcceptedEventDto
                {
                    task_id = taskid,
                    offer_id = offerid,
                    tasker_id = tasker_id,
                    matched_at = taskMatch.matched_at,
                    confirm_expires = taskMatch.confirm_expires
                };

                // broadcast to viewers of this task page
                await _hub.Clients.Group(HubGroups.Task(taskid)).SendAsync(HubEvents.TaskOfferAccepted, acceptedDto);
                _log.LogInformation("Broadcase Event = {Event}, Group = {Group}", HubEvents.TaskOfferAccepted, HubGroups.Task(taskid));

                return result;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _log.LogWarning(e, "Failed to broadcast task.match.created for task {taskid}", taskid);
                throw;
            }

        }

        public async Task<IEnumerable<TaskOffers>> GetTaskOffersAsync(Guid taskid)
        {
            return await _context.task_offers.Where(t => t.task_id == taskid).ToListAsync();
        }

        public async Task<IEnumerable<TaskOffers>> GetTaskOffersByTaskIdAsync(Guid taskid)
        {
            var offers = await _context.task_offers.Include(u => u.user).Where(t => t.task_id == taskid).ToListAsync();
            if(offers is not null)
            {
                offers = offers.GroupBy(o => o.user_id).Select(g => g.OrderByDescending(o => o.created_at).First()).ToList();
            }
            return offers;
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
