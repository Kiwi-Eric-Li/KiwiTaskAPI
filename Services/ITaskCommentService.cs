using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Services
{
    public interface ITaskCommentService
    {
        Task<int> CreateTaskCommentAsync(TaskCommentsDto dto);
        Task<int> ReplyTaskCommentAsync(TaskCommentsDto dto);
    }
}
