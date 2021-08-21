using Ardalis.GuardClauses;
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

namespace Clean.Architecture.Application.Projects.Commands.CompleteProjectTodoItem
{
    public class CompleteProjectTodoItemCommand :IRequest
    {
        public int ProjectId { get; set; } = 0;
        public int ItemId { get; set; } = 0;
    }

    public class CompleteProjectTodoItemCommandHandler : IRequestHandler<CompleteProjectTodoItemCommand>
    {
        private readonly IRepository<Project> _projectRepository;
        public CompleteProjectTodoItemCommandHandler(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<Unit> Handle(CompleteProjectTodoItemCommand request, CancellationToken cancellationToken)
        {
            var projectSpec = new ProjectByIdWithItemsSpec(request.ProjectId);
            var project = await _projectRepository.GetBySpecAsync(projectSpec);
            if (project == null) throw new NotFoundException(nameof(project), request.ProjectId.ToString());

            var toDoItem = project.Items.FirstOrDefault(item => item.Id == request.ItemId);
            if (toDoItem == null) throw new NotFoundException(nameof(toDoItem), request.ItemId.ToString());

            toDoItem.MarkComplete();
            await _projectRepository.UpdateAsync(project);

            return Unit.Value;
        }
    }
}
