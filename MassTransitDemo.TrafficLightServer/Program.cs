using Topshelf;

namespace MassTransitDemo.TrafficLightServer
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(config => config.Service(x => new Service()));
        }
    }
}
