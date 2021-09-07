using Ardalis.Specification;
using Clean.Architecture.Core.ProjectAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Application.CustomSpecification
{

    public class ProjectSampleDto
    {
        public string MyProperty { get; set; }
        public int MyProperty1 { get; set; }
    }

    public class ProjectProjectionSpec : Specification<Project, ProjectSampleDto>
    {
        public ProjectProjectionSpec(int projectId)
        {
            Query
                //.Select(x => new { x.Name, x.Id })
                .Select(x => new ProjectSampleDto { MyProperty = x.Name, MyProperty1 = x.Name.Length })
                .Where(project => project.Id == projectId)
                .Include(project => project.Items);
        }

    }
}
