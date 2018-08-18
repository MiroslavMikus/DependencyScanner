using DependencyScanner.Core.FileScan;
using DependencyScanner.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace DependencyScanner.Core.Nuspec
{
    public class NuspecComparer
    {
        public IEnumerable<ProjectNuspecResult> ConsolidateSolution(SolutionResult solution)
        {
            var projectsToCheck = solution.Projects.Where(a => a.HasNuspec && a.References.Count() > 0);

            foreach (var project in projectsToCheck)
            {
                var docu = ProjectReader.GetDocument(project.NuspecInfo.FullName);

                var dependencies = NuspecUpdater.GetDependencies(docu);

                var missing = CheckMissingPackages(dependencies, ParseReferences(project.References));

                var useless = CheckUselessPackages(dependencies, ParseReferences(project.References));

                if (missing.Any() || useless.Any())
                {
                    yield return new ProjectNuspecResult
                    {
                        Project = project,
                        MissingPackages = missing,
                        UselessPackages = useless
                    };
                }
            }
        }

        /// <summary>
        /// check if project has more references like nuspec
        /// </summary>
        /// <param name="nuspec"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        internal IEnumerable<string> CheckMissingPackages(IEnumerable<string> nuspec, IEnumerable<string> project)
        {
            return project.Except(nuspec);
        }

        /// <summary>
        /// check if nuspec has more references than necessary
        /// </summary>
        /// <param name="nuspec"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        internal IEnumerable<string> CheckUselessPackages(IEnumerable<string> nuspec, IEnumerable<string> project)
        {
            return nuspec.Except(project);
        }

        internal IEnumerable<string> ParseReferences(IEnumerable<ProjectReference> references) => references.Select(a => a.Id);
    }
}
