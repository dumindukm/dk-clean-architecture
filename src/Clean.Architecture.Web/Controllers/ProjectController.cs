using System.Linq;
using System.Threading.Tasks;
using Clean.Architecture.Application.Projects.Queries;
using Clean.Architecture.Application.Projects.Queries.GetProjectByIdWithItems;
using Clean.Architecture.Core;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Core.ProjectAggregate.Specifications;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.Web.ApiModels;
using Clean.Architecture.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Web.Controllers
{
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        private readonly ISender mediator;

        public ProjectController(ISender sender) => mediator = sender;

        // GET project/{projectId?}
        [HttpGet("{projectId:int}")]
        public async Task<IActionResult> Index(int projectId = 1)
        {
            var query = new GetProjectByIdWithItemsQuery{ ProjectId = projectId };
            var project = await mediator.Send(query);

            var dto = new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Items = project.Items
                            .Select(item => ToDoItemViewModel.FromToDoItem(item))
                            .ToList()
            };
            return View(dto);
        }
    }
}
