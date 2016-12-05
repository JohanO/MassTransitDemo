using System.Collections.Generic;

using MassTransitDemo.TrafficLightServer.Domain;

namespace MassTransitDemo.TrafficLightServer.Persistance
{
    public interface IRepository
    {
        bool Exists(int trafficLightId);

        TrafficLight FindById(int trafficLightId);

        IEnumerable<TrafficLight> FindAll();

        void Insert(TrafficLight trafficLight);

        void Update(int trafficLightId, State newState);
    }
}