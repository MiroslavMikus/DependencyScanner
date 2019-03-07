namespace DependencyScanner.Api.Interfaces
{
    public interface IHasInternetConnection : IService
    {
        bool CheckInternetConnection();
    }
}
