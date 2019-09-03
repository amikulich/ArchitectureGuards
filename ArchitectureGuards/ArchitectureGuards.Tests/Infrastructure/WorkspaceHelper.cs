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

            var notLoadedProjects = workspace.CurrentSolution.Projects
                .Where(p => !p.HasDocuments).Select(p => p.Name).ToList();
            Assert.That(notLoadedProjects.Count == 0, 
                $"The following projects have been loaded incorrectly: {string.Join(Environment.NewLine, notLoadedProjects)} {Environment.NewLine} See workspace.Diagnostics for details. ");

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
