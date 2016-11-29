using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MassTransit;

using MassTransitDemo.Contract;
using MassTransitDemo.TrafficLightServer.Domain;
using MassTransitDemo.TrafficLightServer.Persistance;

namespace MassTransitDemo.TrafficLightServer.Handlers
{
    public class StateQueryConsumer : IConsumer<IStateQuery>
    {
        private readonly IRepository _repository;

        public StateQueryConsumer(IRepository repository)
        {
            _repository = repository;
        }

        public Task Consume(ConsumeContext<IStateQuery> context) =>
            context.RespondAsync<IStateQueryResult>(new
            {
                States = _repository.FindAll().Select(x => new TrafficLightState(x))
            });

        private class TrafficLightState : ITrafficLightState
        {
            public TrafficLightState(TrafficLight light)
            {
                Id = light.Id;
                switch (light.State)
                {
                    case State.Initial:
                        LightsOn = Enumerable.Empty<string>();
                        break;

                    case State.Red:
                        LightsOn = new[] { "Red" };
                        break;

                    case State.RedYellow:
                        LightsOn = new[] { "Red", "Yellow" };
                        break;

                    case State.Green:
                        LightsOn = new[] { "Green" };
                        break;
                    case State.Yellow:
                        LightsOn = new[] { "Yellow" };
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public int Id { get; }

            public IEnumerable<string> LightsOn { get; }
        }
    }
}