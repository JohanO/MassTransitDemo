using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Automatonymous;

using MassTransit.Saga;

using MassTransitDemo.Contract;

namespace MassTransitDemo.TrafficLightServer.Automatonymous
{
    public class TrafficLightSaga : MassTransitStateMachine<TrafficLight>
    {
        public TrafficLightSaga()
        {
            InstanceState(x => x.CurrentState);
            Event(() => Create, x => x.CorrelateById(l => l.Id, ctx => ctx.Message.TrafficLightId).SelectId(ctx => Guid.NewGuid()));
            Event(() => GoGreen, x => x.CorrelateById(l => l.Id, ctx => ctx.Message.TrafficLightId));
            Event(() => GoRed, x => x.CorrelateById(l => l.Id, ctx => ctx.Message.TrafficLightId));

            Schedule(() => RedYellowExpired, l => l.ExpirationId, x =>
                {
                    x.Delay = TimeSpan.FromSeconds(2);
                    x.Received = e => e.CorrelateById(l => l.Id, ctx => ctx.Message.TrafficLightId);
                } );
            Schedule(() => YellowExpired, l => l.ExpirationId, x =>
                {
                    x.Delay = TimeSpan.FromSeconds(4);
                    x.Received = e => e.CorrelateById(l => l.Id, ctx => ctx.Message.TrafficLightId);
                });

            Initially(
                When(Create)
                .ThenAsync(ctx => Console.Out.WriteLineAsync($"Created traffic light {ctx.Data.TrafficLightId}"))
                .Publish(ctx => new TrafficLightCreatedEvent(ctx.Data))
                .TransitionTo(Red));

            WhenEnter(Red, x => x.Publish(new LightChangedEvent
                {
                    Color = Red.Name,
                }));
            During(Red,
                When(GoGreen)
                .ThenAsync(ctx => Console.Out.WriteLineAsync($"Traffic light {ctx.Data.TrafficLightId} going green")));
                
        }

        public Guid CorrelationId { get; set; }

        #region States

        public State Red { get; set; }
        public State RedYellow { get; set; }
        public State Green { get; set; }
        public State Yellow { get; set; }

        #endregion

        #region Events

        public Event<ICreateTrafficLightCommand> Create { get; set; }
        public Event<IGoCommand> GoGreen { get; set; }
        public Event<IStopCommand> GoRed { get; set; }

        #endregion

        public Schedule<TrafficLight, IRedYellowExpired> RedYellowExpired { get; set; }
        public Schedule<TrafficLight, IYellowExpired> YellowExpired { get; set; }

        public interface IRedYellowExpired
        {
            int TrafficLightId { get; }
        }

        public interface IYellowExpired
        {
            int TrafficLightId { get; }
        }

        private class TrafficLightCreatedEvent : ITrafficLightCreatedEvent
        {
            public TrafficLightCreatedEvent(ICreateTrafficLightCommand cmd)
            {
                TrafficLightId = cmd.TrafficLightId;
                CorrelationId = cmd.CorrelationId;
            }

            public int TrafficLightId { get; }

            public Guid EventId { get; } = Guid.NewGuid();

            public Guid CorrelationId { get; }
        }

        private class LightChangedEvent : ILightChangedEvent
        {
            public Guid EventId { get; set; }

            public Guid CorrelationId { get; set; }

            public int TrafficLightId { get; set; }

            public string Color { get; set; }

            public bool IsOn { get; set; }
        }
    }
}
