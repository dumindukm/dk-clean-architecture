using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.SharedKernel.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clean.Architecture.Application.Projects.Queries.GetProjects
{
    public class GetProjectsQuery : IRequest<IEnumerable<Project>>
    {
    }

    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IEnumerable<Project>>
    {
        private readonly IRepository<Project> _projectRepository;
        public GetProjectsQueryHandler(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<IEnumerable<Project>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            return await _projectRepository.ListAsync();
        }
    }
}
