using DependencyScanner.Api.Interfaces;
using System.Net;

namespace DependencyScanner.Core.Services
{
    public class HasInternetConnection : IHasInternetConnection
    {
        public bool CheckInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
