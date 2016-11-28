namespace MassTransitDemo.Contract
{
    public interface ILightChangedEvent : IEvent
    {
        int TrafficLightId { get; }

        string Color { get; }

        bool IsOn { get; }
    }
}