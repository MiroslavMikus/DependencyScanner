using DependencyScanner.Core.Model;
using NuGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Core
{
    public class SolutionComparer
    {
        public IEnumerable<ConsolidateReference> FindConsolidateReferences(IEnumerable<SolutionResult> solutions)
        {
            return FindConsolidateReferences(solutions.ToArray());
        }

        public IEnumerable<ConsolidateReference> FindConsolidateReferences(params SolutionResult[] solutions)
        {
            var allReferenes = solutions.SelectMany(a => a.GetSolutionReferences()).GroupBy(a => a.Id);

            foreach (var reference in allReferenes)
            {
                var allVersions = reference.Select(a => a.Version).ToList();

                if (AllAreSame(allVersions)) continue;

                var packageId = reference.First().Id;

                var occuringSolutions = solutions.Where(a => a.GetSolutionReferences().Any(b => b.Id == packageId));

                var dict = occuringSolutions.ToDictionary(a => a, b => b.GetSolutionReferences().First(a => a.Id == packageId).Version);

                yield return new ConsolidateReference
                {
                    Id = packageId,
                    References = dict
                };
            }
        }

        internal bool AllAreSame(IEnumerable<SemanticVersion> versions)
        {
            if (versions.Count() == 1) return true;

            foreach (var versionSource in versions)
            {
                foreach (var versionTarget in versions)
                {
                    if (versionSource != versionTarget)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
