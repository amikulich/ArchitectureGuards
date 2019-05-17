using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ArchitectureGuards.Tests.Infrastructure;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace ArchitectureGuards.Tests.DependencyGraph
{
    [TestFixture]
    public class DependencyGraphTests
    {
        Workspace workspace;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            workspace = await WorkspaceHelper.Load("SampleProject1ForDependencyGraph.sln");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            workspace.Dispose();
        }

        [Test]
        public async Task DependencyGraph_WhenADocumentContainsOnlyOneClassOrInterface_IsBuildCorrectly()
        {
            var dependencies = new Dictionary<string, List<string>>(); 

            var project = workspace.CurrentSolution.Projects.First();

            foreach (var document in project.Documents)
            {
                var semanticModel = await document.GetSemanticModelAsync();
                KeyValuePair<string, List<string>>? keyValue = null;

                foreach (var item in semanticModel.SyntaxTree.GetRoot().DescendantNodes())
                {
                    switch (item)
                    {
                        case ClassDeclarationSyntax classDeclaration:
                        case InterfaceDeclarationSyntax interfaceDeclaration:
                            if (!keyValue.HasValue)
                            {
                                keyValue = new KeyValuePair<string, List<string>>(semanticModel.GetDeclaredSymbol(item).Name, new List<string>());
                            }
                            break;
                        case SimpleBaseTypeSyntax simpleBaseTypeSyntax:
                            keyValue?.Value.Add(simpleBaseTypeSyntax.Type.ToString());
                            break;
                        case ParameterSyntax parameterSyntax:
                            keyValue?.Value.Add(parameterSyntax.Type.ToString());
                            break;
                    }
                }

                if (keyValue.HasValue)
                {
                    dependencies.Add(keyValue.Value.Key, keyValue.Value.Value);
                }
            }

            Assert.That(dependencies.Count == 4);
            
            var dogDeps = dependencies["Dog"];
            Assert.IsTrue(dogDeps.Exists(dependency => dependency == "IBark"));
            Assert.IsTrue(dogDeps.Exists(dependency => dependency == "Cat"));
            Assert.IsTrue(dogDeps.Exists(dependency => dependency == "Animal"));
        }
    }
}
