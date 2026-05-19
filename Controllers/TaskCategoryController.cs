using AutoMapper;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KiwiTaskAPI.Controllers
{
    [Route("api/task-category")]
    [ApiController]
    public class TaskCategoryController : ControllerBase
    {
        private ITaskCategoryRepository _taskCategoryRepository;
        private readonly IMapper _mapper;
        
        public TaskCategoryController(ITaskCategoryRepository taskCategoryRepository, IMapper mapper)
        {
            _taskCategoryRepository = taskCategoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTaskCategories()
        {
            var taskCategoryRepo = await _taskCategoryRepository.GetTaskCategoriesAsync();
            if(taskCategoryRepo == null || taskCategoryRepo.Count() < 0)
            {
                return NotFound("Task categories cannot be found.");
            }
            var taskCategoryDto = _mapper.Map<IEnumerable<TaskCategoryDto>>(taskCategoryRepo);
            return Ok(new { 
                code =0,
                data = taskCategoryDto

            });
        }


    }
}
