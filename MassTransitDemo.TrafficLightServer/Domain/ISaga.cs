using System.Threading.Tasks;

namespace MassTransitDemo.TrafficLightServer.Domain
{
    public interface ISaga
    {
        bool CanFire(int trafficLightId, Trigger trigger);

        Task FireAsync(int trafficLightId, Trigger trigger);
    }
}