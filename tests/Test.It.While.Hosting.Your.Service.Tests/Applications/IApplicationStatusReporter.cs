namespace Test.It.While.Hosting.Your.Service.Tests.Applications
{
    public interface IApplicationStatusReporter
    {
        bool HaveStarted { get; set; }
    }
}