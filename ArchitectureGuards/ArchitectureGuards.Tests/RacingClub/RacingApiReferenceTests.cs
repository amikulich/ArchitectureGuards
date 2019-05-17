using System.Linq;
using System.Threading.Tasks;

using ArchitectureGuards.Tests.Infrastructure;

using Microsoft.CodeAnalysis;

using NUnit.Framework;

namespace ArchitectureGuards.Tests.RacingClub
{
    [TestFixture]
    public class RacingApiReferenceTests
    {
        Workspace _workspace;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            _workspace = await WorkspaceHelper.Load("RacingClub.sln");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _workspace.Dispose();
        }

        [Test]
        public void DomainModel_CannotReferenceOtherLibraries()
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

        [Test]
        [TestCase(".DataAccess")]
        [TestCase(".DomainModel")]
        public void ApiProject_DoesNotReferenceTheseProjects(string mask)
        {
            const string apiProjectName = "RacingClub.Api";

            Project apiProject =
                _workspace.CurrentSolution.Projects.Single(p => string.Equals(p.Name, apiProjectName));

            var undesiredProjects =
                _workspace.CurrentSolution.Projects.Where(p => p.Name.EndsWith(mask)).ToList();

            Assert.IsNotEmpty(undesiredProjects, $"None of projects with the mask {mask} was found. They might have been removed or renamed.");

            foreach (var reference in apiProject.ProjectReferences)
            {
                var referencedProject = _workspace.CurrentSolution.GetProject(reference.ProjectId);

                foreach (var undesiredProject in undesiredProjects)
                {
                    if (undesiredProject == referencedProject)
                    {
                        Assert.Fail(
                            $"Architecture violation: {apiProjectName} cannot reference domain or data access libraries directly. Please remove {undesiredProject.Name} references from {apiProject}");
                    }
                }
            }
        }
    }
}
