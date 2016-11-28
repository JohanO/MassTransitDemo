using System.Collections.Generic;

namespace MassTransitDemo.TrafficLightServer.Stateless
{
    public static class Repository
    {
        public static Dictionary<int, TrafficLight> TrafficLights { get; } = new Dictionary<int, TrafficLight>();
    }
}