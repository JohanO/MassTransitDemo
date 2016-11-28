namespace MassTransitDemo.Contract
{
    public interface IGoRedCommand : ICommand
    {
        int TrafficLightId { get; }
    }
}