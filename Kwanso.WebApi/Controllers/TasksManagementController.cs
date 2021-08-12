using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kwanso.Model.Poco;
using Kwanso.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kwanso.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksManagement : ControllerBase
    {
        private ITaskRepository taskRepository;

        public TasksManagement(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        [HttpGet("/list-tasks")]
        [ProducesResponseType(200, Type = typeof(Users))]
        public IActionResult GetAll()
        {
            var listtasks = taskRepository.GetAll();
            return Ok(listtasks);
        }

        [HttpPost("/create-task")]
        [ProducesResponseType(200, Type = typeof(Tasks))]
        [Produces("application/json")]
        public IActionResult Create([FromBody] Tasks tasks)
        {
            var usertask = taskRepository.Create(tasks);
            return Ok(usertask);
        }

        [HttpPost("/bulk-delete")]
        [ProducesResponseType(200, Type = typeof(Tasks))]
        [Produces("application/json")]
        public IActionResult Delete([FromBody] List<int> Id)
        {
            var tasks = taskRepository.Delete(Id);
            return Ok(tasks);
        }
    }
}