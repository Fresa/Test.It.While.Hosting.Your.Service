using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Service.Delegates
{
    public delegate Task StopAsync(CancellationToken cancellationToken);
}