using DependencyScanner.Core.Interfaces;
using DependencyScanner.Core.Model;
using System.Collections.Generic;
using System.Linq;
using static DependencyScanner.Core.Tools.VersionComparer;

namespace DependencyScanner.Core
{
    public class ProjectComparer : IService
    {
        public IEnumerable<ConsolidateProject> FindConsolidateReferences(SolutionResult solution)
        {
            foreach (var reference in solution.GetSolutionReferences().GroupBy(a => a.Id))
            {
                var allVersions = reference.Select(a => a.Version).ToList();

                if (AllAreSame(allVersions)) continue;

                var packageId = reference.First().Id;

                var ocurringProjects = solution.Projects.Where(a => a.References.Any(b => b.Id == packageId));

                var dict = ocurringProjects.ToDictionary(a => a, b => b.References.First(c => c.Id == packageId).Version);

                yield return new ConsolidateProject
                {
                    Id = packageId,
                    References = dict
                };
            }
        }
    }
}
