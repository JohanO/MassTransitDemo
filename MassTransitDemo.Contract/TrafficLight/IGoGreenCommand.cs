using System;

namespace MassTransitDemo.Contract
{
    public interface IGoGreenCommand : ICommand
    {
        int TrafficLightId { get; }
    }
}