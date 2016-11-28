using System;

using Automatonymous;

namespace MassTransitDemo.TrafficLightServer.Automatonymous
{
    public class TrafficLight : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public int Id { get; set; }
        public string CurrentState { get; set; }

        public Guid? ExpirationId { get; set; }
    }
}