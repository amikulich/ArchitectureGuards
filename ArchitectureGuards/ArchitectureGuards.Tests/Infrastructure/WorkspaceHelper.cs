using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using NUnit.Framework;

namespace ArchitectureGuards.Tests.Infrastructure
{
    public static class WorkspaceHelper
    {
        /// <param name="solutionName">Name of a solution file - with or without .sln extension</param>
        public static async Task<Workspace> Load(string solutionName)
        {
            var solutionFilePath = GetSolutionFilePath(solutionName);

            var workspace = MSBuildWorkspace.Create();

            await workspace.OpenSolutionAsync(solutionFilePath);

            Assert.That(workspace.CurrentSolution.Projects.All(p => p.HasDocuments), $"The following projects have been loaded incorrectly: {string.Join(Environment.NewLine, workspace.CurrentSolution.Projects.Where(p => !p.HasDocuments).Select(p => p.Name))} {Environment.NewLine} See workspace.Diagnostics for details. ");

            return workspace;
        }

        private static string GetSolutionFilePath(string solutionName)
        {
            string testAssemblyPath = Assembly.GetExecutingAssembly().Location;
            string repositoryPathRoot = testAssemblyPath.Substring(0, testAssemblyPath.IndexOf("\\ArchitectureGuards", StringComparison.Ordinal));


            return Directory.GetFiles(repositoryPathRoot, $"{TrailingSolutionFileExtension.Replace(solutionName, string.Empty)}.sln", SearchOption.AllDirectories).SingleOrDefault();
        }

        private static Regex TrailingSolutionFileExtension => new Regex(@"\.sln$", RegexOptions.IgnoreCase);

    }
}
