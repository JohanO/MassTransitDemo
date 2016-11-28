using Topshelf;

namespace MassTransitDemo.TrafficLightServer.Automatonymous
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(config => config.Service(x => new TrafficLightService()));
        }
    }
}
