using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using MassTransit;

using MassTransitDemo.Contract;

namespace MassTransitDemo.TrafficLightServer.Stateless
{
    public class GoCommandConsumer : IConsumer<IGoCommand>
    {
        private readonly IBusControl _bus;

        public GoCommandConsumer(IBusControl bus)
        {
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<IGoCommand> context)
        {
            var cmd = context.Message;
            await Console.Out.WriteLineAsync($"Received IGoCommand: Id={cmd.TrafficLightId};");

            if (!Repository.TrafficLights.ContainsKey(cmd.TrafficLightId))
            {
                await context.RespondAsync<ICommandValidationResult>(new
                {
                    cmd.CommandId,
                    cmd.CorrelationId,
                    Fail = true,
                    Message = $"TrafficLight {cmd.TrafficLightId} does not exist"
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

            var trafficLight = Repository.TrafficLights[cmd.TrafficLightId];
            var saga = new Saga(_bus, trafficLight);
            await saga.StateMachine.FireAsync(Trigger.ToRedYellow);
            Repository.TrafficLights[cmd.TrafficLightId] = trafficLight;
        }
    }
}