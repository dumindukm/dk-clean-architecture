using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.SharedKernel.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace Clean.Architecture.Application.Projects.Commands.CreateProject
{
    public class CreateProjectCommand : IRequest<Project>
    {
        public string Name { get; set; }
    }

    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Project name is empty");
        }
    }

    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Project>
    {
        private readonly IRepository<Project> _projectRepository;
        public CreateProjectCommandHandler(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var newProject = new Project(request.Name);

            var createdProject = await _projectRepository.AddAsync(newProject);
            await _projectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return createdProject;
        }
    }
}

