using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Test.It.While.Hosting.Your.Service.Tests.Specifications
{
    public abstract class ExceptionSupressingServiceSpecification<TConfiguration> : ServiceSpecification<TConfiguration>, IAsyncLifetime
        where TConfiguration : class, IServiceHostStarter, new()
    {
        private TConfiguration _configuration;

        protected abstract void OnException(Exception exception);

        public async Task InitializeAsync()
        {
            _configuration = new TConfiguration();
            await SetConfigurationAsync(_configuration)
                .ContinueWith(task => task.Exception?.Flatten().InnerExceptions.ToList().ForEach(OnException));
        }

        public Task DisposeAsync()
        {
            _configuration.Dispose();
            return Task.CompletedTask;
        }
    }
}