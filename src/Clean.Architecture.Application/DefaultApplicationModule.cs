using Autofac;
using Clean.Architecture.Application.Projects.Commands.CompleteProjectTodoItem;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Clean.Architecture.Application.Validation;

namespace Clean.Architecture.Application
{
    public class DefaultApplicationModule :Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
               .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(CompleteProjectTodoItemCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));
            builder.RegisterAssemblyTypes(typeof(CompleteProjectTodoItemCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IValidator<>));
            builder.RegisterGeneric(typeof(CommandValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}
