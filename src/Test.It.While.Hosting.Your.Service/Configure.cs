using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Service
{
    public delegate Task Configure(IServiceContainer serviceContainer, CancellationToken cancellationToken = default);
}