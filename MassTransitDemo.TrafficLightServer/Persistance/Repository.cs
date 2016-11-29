using System.Collections.Generic;

using MassTransitDemo.TrafficLightServer.Domain;

namespace MassTransitDemo.TrafficLightServer.Persistance
{
    public class Repository : IRepository
    {
        private readonly Dictionary<int, TrafficLight> _trafficLights = new Dictionary<int, TrafficLight>();

        public bool Exists(int id) => _trafficLights.ContainsKey(id);

        public TrafficLight FindById(int id) => _trafficLights.ContainsKey(id) ? _trafficLights[id] : null;

        public void Save(TrafficLight trafficLight) => _trafficLights[trafficLight.Id] = trafficLight;

        public IEnumerable<TrafficLight> FindAll() => _trafficLights.Values;
    }
}