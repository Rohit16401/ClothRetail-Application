using Domain.Repositories;

namespace Domain.Services.HelperServices
{
    public class ConnectionStringBuilder : IConnectionStringBuilder
    {
        public string SqlConnectionString { get; set; }
    }
}
