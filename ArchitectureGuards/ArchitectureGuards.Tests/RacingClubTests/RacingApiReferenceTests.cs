using System.Linq;
using ArchitectureGuards.Tests.Infrastructure;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace ArchitectureGuards.Tests.RacingClubTests
{
    [TestFixture]
    public class RacingApiReferenceTests
    {
        Workspace _workspace;

        [OneTimeSetUp]
        public void SetUp()
        {
            _workspace = WorkspaceHelper.Load("");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _workspace.Dispose();
        }

        [Test]
        public void MethodUnderTest_ConditionBeingTested_ExpectedResult()
        {
            var domainModelProjects =
                _workspace.CurrentSolution.Projects.Where(p => p.Name.EndsWith(".DomainModel")).ToList();

            foreach (var project in domainModelProjects)
            {
                foreach (var reference in project.ProjectReferences)
                {
                    var referencedProject = _workspace.CurrentSolution.GetProject(reference.ProjectId);

                    Assert.Fail(
                        $"Architecture violation: {project.Name} is a domain model library but references {referencedProject.Name}. You must remove this reference before proceeding.");
                }
            }

        }
    }
}
