using System.Threading.Tasks;
using Xunit;

namespace Test.It.While.Hosting.Your.Service.Tests.Specifications
{
    public abstract class XUnitServiceSpecification<TConfiguration> : ServiceSpecification<TConfiguration>, IAsyncLifetime
        where TConfiguration : class, IServiceHostStarter, new()
    {
        private TConfiguration _configuration;

        public async Task InitializeAsync()
        {
            _configuration = new TConfiguration();
            await SetConfigurationAsync(_configuration);
        }

        public Task DisposeAsync()
        {
            _configuration.Dispose();
            return Task.CompletedTask;
        }
    }
}