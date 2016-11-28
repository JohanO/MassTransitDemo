using System;
using System.Threading.Tasks;

using MassTransit;

using MassTransitDemo.Contract;

namespace MassTransitDemo.TrafficLightServer.Stateless
{
    public class CreateTrafficLightCommandConsumer : IConsumer<ICreateTrafficLightCommand>
    {
        private readonly IBusControl _bus;

        public CreateTrafficLightCommandConsumer(IBusControl bus)
        {
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<ICreateTrafficLightCommand> context)
        {
            var cmd = context.Message;
            await Console.Out.WriteLineAsync($"Received ICreateTrafficLightCommand: Id={cmd.TrafficLightId};");

            if (Repository.TrafficLights.ContainsKey(cmd.TrafficLightId))
            {
                await context.RespondAsync<ICommandValidationResult>(new
                {
                    cmd.CommandId,
                    cmd.CorrelationId,
                    Fail = false,
                    Message = "Already exist"
                });
                return;
            }

            await context.RespondAsync<ICommandValidationResult>(new
            {
                cmd.CommandId,
                cmd.CorrelationId,
                Fail = false,
                Message = "Validated OK"
            });

            var trafficLight = new TrafficLight { Id = cmd.TrafficLightId };
            var saga = new Saga(_bus, trafficLight);
            await saga.StateMachine.FireAsync(Trigger.ToRed);
            Repository.TrafficLights[cmd.TrafficLightId] = trafficLight;
        }
    }
}