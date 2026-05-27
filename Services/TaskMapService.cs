using KiwiTaskAPI.Database;
using KiwiTaskAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace KiwiTaskAPI.Services
{
    public class TaskMapService: ITaskMapService
    {
        private static readonly GeoBounds NzDefaultBounds = new(165, -48, 179.7, -34);
        private const int DefaultLimit = 500;
        private const int MinLimit = 50;
        private const int MaxLimit = 1000;
        private readonly AppDbContext _context;

        public TaskMapService(AppDbContext context)
        {
            _context = context;
        }

        private static int Clamp(int value, int min, int max) =>
            value < min ? min : (value > max ? max : value);

        public Task<IEnumerable<TaskMapPinDto>> GetOpenPinsAsync(MapPinsRequest request)
        {
            throw new NotImplementedException();
        }


        //public async Task<IEnumerable<TaskMapPinDto>> GetOpenPinsAsync(MapPinsRequest request)
        //{
        //    var bounds = request.Bounds;
        //    bool isZeroBounds = bounds.West == 0 && bounds.East == 0 && bounds.South == 0 && bounds.North == 0;
        //    bool isInvalid =
        //        Math.Abs(bounds.West) > 180 || Math.Abs(bounds.East) > 180 ||
        //        Math.Abs(bounds.South) > 90 || Math.Abs(bounds.North) > 90;
        //    if (isZeroBounds || isInvalid)
        //    {
        //        bounds = NzDefaultBounds;
        //    }

        //    var limit = Clamp(request.Limit ?? DefaultLimit, MinLimit, MaxLimit);
        //    var now = DateTime.UtcNow;

        //    decimal west = (decimal)bounds.West;
        //    decimal south = (decimal)bounds.South;
        //    decimal east = (decimal)bounds.East;
        //    decimal north = (decimal)bounds.North;

        //    var baseQuery = _context.tasks.AsNoTracking()
        //        .Where(t => 
        //            t.status == "Open" && 
        //            t.task_type == 0 && 
        //            t.expires_at > now && 
        //            t.latitude.HasValue && 
        //            t.longitude.HasValue &&
        //            t.latitude >= south && 
        //            t.latitude <= north &&
        //            t.longitude >= west &&
        //            t.longitude <= east
        //        );
        //}
    }
}
