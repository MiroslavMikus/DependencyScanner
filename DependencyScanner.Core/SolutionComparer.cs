using DependencyScanner.Core.Interfaces;
using DependencyScanner.Core.Model;
using System.Collections.Generic;
using System.Linq;
using static DependencyScanner.Core.Tools.VersionComparer;

namespace DependencyScanner.Core
{
    public class SolutionComparer : IService
    {
        public IEnumerable<ConsolidateSolution> FindConsolidateReferences(IEnumerable<SolutionResult> solutions)
        {
            return FindConsolidateReferences(solutions.ToArray());
        }

        public IEnumerable<ConsolidateSolution> FindConsolidateReferences(params SolutionResult[] solutions)
        {
            var allReferenes = solutions.SelectMany(a => a.GetSolutionReferences()).GroupBy(a => a.Id);

            foreach (var reference in allReferenes)
            {
                var allVersions = reference.Select(a => a.Version).ToList();

                if (AllAreSame(allVersions)) continue;

                var packageId = reference.First().Id;

                var occuringSolutions = solutions.Where(a => a.GetSolutionReferences().Any(b => b.Id == packageId));

                var dict = occuringSolutions.ToDictionary(a => a, b => b.GetSolutionReferences().First(a => a.Id == packageId).Version);

                yield return new ConsolidateSolution
                {
                    Id = packageId,
                    References = dict
                };
            }
        }
    }
}
