using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Core.ProjectAggregate.Specifications;
using Clean.Architecture.SharedKernel.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clean.Architecture.Application.Projects.Queries.GetProjectByIdWithItems
{
    public class GetProjectByIdWithItemsQuery : IRequest<Project>
    {
        public int ProjectId { get; set; } = 0;
    }

    public class GetProjectByIdWithItemsHandler : IRequestHandler<GetProjectByIdWithItemsQuery, Project>
    {
        private readonly IRepository<Project> _projectRepository;
        public GetProjectByIdWithItemsHandler(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Project> Handle(GetProjectByIdWithItemsQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProjectByIdWithItemsSpec(request.ProjectId);
            var project = await _projectRepository.GetBySpecAsync(spec);

            return project;

        }
    }

}
