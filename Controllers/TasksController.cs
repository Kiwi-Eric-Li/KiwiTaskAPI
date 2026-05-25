using KiwiTaskAPI.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using KiwiTaskAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.JSInterop.Infrastructure;
using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TasksController(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks([FromQuery] int page_num = 1, [FromQuery] int page_size = 10, [FromQuery] string? title = null)      // IActionResult returns a response of HTTP
        {
            var (tasksRepo, totalCount) = await _taskRepository.GetTasksAsync(page_num, page_size, title);
            if(tasksRepo == null || tasksRepo.Count() < 0)
            {
                return NotFound("There are no tasks.");
            }
            var tasksDtoRepo = _mapper.Map<IEnumerable<TasksDto>>(tasksRepo);
            return Ok(new { 
                code = 0,
                data = tasksDtoRepo,
                pagination = new
                {
                    pageNum = page_num,
                    pageSize = page_size,
                    totalCount = totalCount,
                    totalPages = (int)Math.Ceiling(totalCount / (double)page_size)
                }
            });
        }

        [HttpGet("some")]
        public async Task<IActionResult> GetTheFirstTask()
        {
            var tasksRepo = await _taskRepository.GetFewTasksAsync();
            if(tasksRepo.Count() < 0)
            {
                return NotFound("There are no tasks.");
            }
            var tasksDtoRepo = _mapper.Map<IEnumerable<TasksDto>>(tasksRepo);
            return Ok(new
            {
                code = 0,
                data = tasksDtoRepo
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var taskRepo = await _taskRepository.GetTaskByIdAsync(id);
            if(taskRepo == null)
            {
                return NotFound("This task cannot be found.");
            }
            var taskDto = _mapper.Map<TasksDto>(taskRepo);
            return Ok(taskDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTask([FromBody] TasksDto dto)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            dto.poster_id = Guid.Parse(userId);

            dto.id = Guid.NewGuid();
            if (!string.IsNullOrWhiteSpace(dto.location))
            {
                string[] result = dto.location.Split(",");
                dto.suburb = result[0].Trim();
                dto.city = result[1].Trim();
                dto.postcode = result[2].Trim();
            }

            var taskEntity = _mapper.Map<Tasks>(dto);
            int saveResult = await _taskRepository.CreateTaskAsync(taskEntity);
            if(saveResult > 0)
            {
                return Ok(new {
                   code = 0,
                   message = "Add task successfully."
                });
            }
            else
            {
                return Ok(new
                {
                    code = -1,
                    message = "Failed to add the task"
                });
            }

        }
    }
}
