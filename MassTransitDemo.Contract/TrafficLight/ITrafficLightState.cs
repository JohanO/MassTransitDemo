using System.Collections.Generic;

namespace MassTransitDemo.Contract
{
    public interface ITrafficLightState
    {
        int Id { get; }

        IEnumerable<string> LightsOn { get; }
    }
}