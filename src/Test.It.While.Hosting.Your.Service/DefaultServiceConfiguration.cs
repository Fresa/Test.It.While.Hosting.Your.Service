namespace Test.It.While.Hosting.Your.Service
{
    internal class DefaultServiceConfiguration : IServiceConfiguration
    {
        public DefaultServiceConfiguration(params string[] startParameters)
        {
            StartParameters = startParameters;
        }

        public string[] StartParameters { get; }
    }
}