using System;
using System.Threading.Tasks;

using MassTransit;

using MassTransitDemo.Contract;
using MassTransitDemo.TrafficLightServer.Domain;
using MassTransitDemo.TrafficLightServer.Persistance;

namespace MassTransitDemo.TrafficLightServer.Handlers
{
    public class StopCommandConsumer : IConsumer<IStopCommand>
    {
        private readonly ISaga _saga;
        private readonly IRepository _repository;

        public StopCommandConsumer(ISaga saga, IRepository repository)
        {
            _saga = saga;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IStopCommand> context)
        {
            var cmd = context.Message;
            await Console.Out.WriteLineAsync($"Received IStopCommand: Id={cmd.TrafficLightId};");

            if (!_repository.Exists(cmd.TrafficLightId))
            {
                await context.RespondResultAsync(true, $"TrafficLight {cmd.TrafficLightId} does not exist");
            }
            else
            {
                await context.RespondResultAsync(false, "Validated OK");

                var trafficLight = _repository.FindById(cmd.TrafficLightId);
                await _saga.FireAsync(trafficLight, Trigger.ToYellow);
                _repository.Save(trafficLight);
            }
        }
    }
}