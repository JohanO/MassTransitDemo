using System.Threading.Tasks;

namespace MassTransitDemo.TrafficLightServer.Domain
{
    public interface ISaga
    {
        Task FireAsync(int trafficLightId, Trigger trigger);
    }
}