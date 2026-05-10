using AutoMapper;
using KiwiTaskAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KiwiTaskAPI.Controllers
{
    [Route("api/tasks/{taskid}/attachment")]
    [ApiController]
    public class TaskAttachmentController: ControllerBase
    {
        private ITaskRepository _taskRepository;
        private IMapper _mapper;

        public TaskAttachmentController(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }


    }
}
