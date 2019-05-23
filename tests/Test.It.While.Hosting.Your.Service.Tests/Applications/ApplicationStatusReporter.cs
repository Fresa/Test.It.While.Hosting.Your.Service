namespace Test.It.While.Hosting.Your.Service.Tests.Applications
{
    internal class ApplicationStatusReporter : IApplicationStatusReporter
    {
        public bool HaveStarted { get => false; set {} }
    }
}