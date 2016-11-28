namespace MassTransitDemo.Contract
{
    public interface ITrafficLightCreatedEvent : IEvent
    {
        int TrafficLightId { get; }
    }
}