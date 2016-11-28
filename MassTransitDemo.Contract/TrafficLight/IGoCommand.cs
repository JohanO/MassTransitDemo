using System;

namespace MassTransitDemo.Contract
{
    public interface IGoCommand : ICommand
    {
        int TrafficLightId { get; }
    }
}