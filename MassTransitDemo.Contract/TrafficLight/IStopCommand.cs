namespace MassTransitDemo.Contract
{
    public interface IStopCommand : ICommand
    {
        int TrafficLightId { get; }
    }
}