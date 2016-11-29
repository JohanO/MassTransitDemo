using System.Collections.Generic;

namespace MassTransitDemo.Contract
{
    public interface IStateQueryResult
    {
        IEnumerable<ITrafficLightState> States { get; }
    }
}