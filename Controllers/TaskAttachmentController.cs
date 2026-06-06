using AutoMapper;
using KiwiTaskAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KiwiTaskAPI.Controllers
{
    [Route("api/tasks/{taskid}/attachment")]
    [ApiController]
    public class TaskAttachmentController: ControllerBase
    {
        private ITaskService _taskRepository;
        private IMapper _mapper;

        public TaskAttachmentController(ITaskService taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }


    }
}
