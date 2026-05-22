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
        public async Task<IActionResult> GetAllTasks()      // IActionResult 返回的是HTTP的响应结果
        {
            var tasksRepo = await _taskRepository.GetTasksAsync();
            if(tasksRepo == null || tasksRepo.Count() < 0)
            {
                return NotFound("There are no tasks.");
            }
            var tasksDto = _mapper.Map<IEnumerable<TasksDto>>(tasksRepo);
            return Ok(tasksDto);
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


            return Ok(saveResult);
        }
    }
}
