using MassTransitDemo.TrafficLightServer.Stateless.Domain;

namespace MassTransitDemo.TrafficLightServer.Stateless.Persistance
{
    public interface IRepository
    {
        bool Exists(int trafficLightId);

        TrafficLight FindById(int trafficLightId);

        void Save(TrafficLight trafficLight);
    }
}