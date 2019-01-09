using DependencyScanner.Api.Model;
using System.Threading.Tasks;

namespace DependencyScanner.Api.Interfaces
{
    public interface IScanner : IService
    {
        Task<WorkingDirectory> ScanWorkingDirectory(string rootDirectory, ICancelableProgress<ProgressMessage> progress, bool executeGitFetch);

        Task<Solution> ScanSolution(string rootDirectory, ICancelableProgress<ProgressMessage> progress, bool executeGitFetch);
    }

    public class ProgressMessage
    {
        public double Value { get; set; }
        public string Message { get; set; }
    }
}
