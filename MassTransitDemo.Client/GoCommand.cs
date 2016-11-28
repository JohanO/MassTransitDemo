using MassTransitDemo.Contract;

namespace MassTransitDemo.Client
{
    internal class GoCommand : Command, IGoCommand
    {
        public int TrafficLightId { get; set; }
    }
}