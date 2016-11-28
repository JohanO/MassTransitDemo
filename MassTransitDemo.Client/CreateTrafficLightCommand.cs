using MassTransitDemo.Contract;

namespace MassTransitDemo.Client
{
    internal class CreateTrafficLightCommand : Command, ICreateTrafficLightCommand
    {
        public int TrafficLightId { get; set; }
    }
}