using MassTransitDemo.TrafficLightServer.Domain;

namespace MassTransitDemo.TrafficLightServer.Persistance
{
    public interface IRepository
    {
        bool Exists(int trafficLightId);

        TrafficLight FindById(int trafficLightId);

        void Save(TrafficLight trafficLight);
    }
}