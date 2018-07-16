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
        public IEnumerable<SolutionResult> FindConsolidateReferences(params SolutionResult[] solutions)
        {
            Dictionary<string, bool> ProcesedPackages = new Dictionary<string, bool>();

            var allPackages = solutions.SelectMany(a => a.Projects).SelectMany(a => a.References);

            foreach (var solution in solutions)
            {
                var resultSolution = new SolutionResult(solution.Info);

                foreach (var package in solution.Projects.SelectMany(a => a.References))
                {
                    var similiarPackages = allPackages.Where(a => a.Id == package.Id);

                    if (!ProcesedPackages.ContainsKey(package.Id))
                    {
                        var consolidation = !similiarPackages.All(a => a.Version.ToString() == package.Version.ToString());

                        ProcesedPackages.Add(package.Id, consolidation);

                        if (consolidation)
                        {
                            // todo resume here
                            // add package to project and project to solution
                        }
                    }
                    else
                    {
                        if (ProcesedPackages[package.Id])
                        {
                            // add package to project and project to solution
                        }
                    }
                }

                yield return resultSolution;
            }
        }
    }

    public class SolutionComparsionResult
    {

    }
}
