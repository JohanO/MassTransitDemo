using System.Threading.Tasks;

namespace MassTransitDemo.TrafficLightServer.Domain
{
    public interface ISaga
    {
        Task FireAsync(TrafficLight trafficLight, Trigger trigger);
    }
}