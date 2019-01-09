using DependencyScanner.Api.Interfaces;

namespace DependencyScanner.Api.Model
{
    public interface IScannable
    {
        void Sync(IScanner scanner);
    }
}
