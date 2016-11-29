namespace MassTransitDemo.TrafficLightServer.Stateless.Domain
{
    public class TrafficLight
    {
        public int Id { get; set; }
        public State State { get; set; } = State.Initial;
    }
}