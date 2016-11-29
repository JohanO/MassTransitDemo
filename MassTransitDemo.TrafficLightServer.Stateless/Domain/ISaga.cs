using System.Threading.Tasks;

namespace MassTransitDemo.TrafficLightServer.Stateless.Domain
{
    public interface ISaga
    {
        Task FireAsync(TrafficLight trafficLight, Trigger trigger);
    }
}