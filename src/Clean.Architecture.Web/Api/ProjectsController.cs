using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clean.Architecture.Application.Projects.Commands.CompleteProjectTodoItem;
using Clean.Architecture.Application.Projects.Commands.CreateProject;
using Clean.Architecture.Application.Projects.Queries;
using Clean.Architecture.Application.Projects.Queries.GetProjectByIdWithItems;
using Clean.Architecture.Application.Projects.Queries.GetProjects;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Core.ProjectAggregate.Specifications;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.Web.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Web.Api
{
    /// <summary>
    /// A sample API Controller. Consider using API Endpoints (see Endpoints folder) for a more SOLID approach to building APIs
    /// https://github.com/ardalis/ApiEndpoints
    /// </summary>
    public class ProjectsController : BaseApiController
    {
        private readonly ISender mediator;

        public ProjectsController(ISender sender) => mediator = sender;

        // GET: api/Projects
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var projectDTOs = (await mediator.Send(new GetProjectsQuery()))
                .Select(project => new ProjectDTO
                {
                    Id = project.Id,
                    Name = project.Name
                })
                .ToList();

            return Ok(projectDTOs);
        }

        // GET: api/Projects
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetProjectByIdWithItemsQuery { ProjectId = id };
            var project = await mediator.Send(query);

            var result = new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                Items = new List<ToDoItemDTO>
                (
                    project.Items.Select(i => ToDoItemDTO.FromToDoItem(i)).ToList()
                )
            };

            return Ok(result);
        }

        // POST: api/Projects
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProjectCommand request)
        {
            var createdProject = await mediator.Send(request);

            var result = new ProjectDTO
            {
                Id = createdProject.Id,
                Name = createdProject.Name
            };
            return Ok(result);
        }

        // PATCH: api/Projects/{projectId}/complete/{itemId}
        [HttpPatch("{projectId:int}/complete/{itemId}")]
        public async Task<IActionResult> Complete(int projectId, int itemId)
        {
            var query = new CompleteProjectTodoItemCommand { ProjectId = projectId, ItemId = itemId };
            await mediator.Send(query);
            return Ok();
        }
    }
}
