namespace MassTransitDemo.TrafficLightServer.Stateless
{
    public class TrafficLight
    {
        public int Id { get; set; }
        public State State { get; set; } = State.Initial;
    }
}