using MassTransitDemo.Contract;

namespace MassTransitDemo.Client
{
    internal class StopCommand : Command, IStopCommand
    {
        public int TrafficLightId { get; set; }
    }
}