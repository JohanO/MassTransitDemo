namespace MassTransitDemo.Contract
{
    public interface ICreateTrafficLightCommand : ICommand
    {
        int TrafficLightId { get; }
    }
}