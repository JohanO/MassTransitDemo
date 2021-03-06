﻿using System;
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
            else if (!_saga.CanFire(cmd.TrafficLightId, Trigger.ToYellow))
            {
                await context.RespondResultAsync(true, $"TrafficLight {cmd.TrafficLightId} in wrong state");
            }
            else
            {
                await context.RespondResultAsync(false, "Validated OK");
                await _saga.FireAsync(cmd.TrafficLightId, Trigger.ToYellow);
            }
        }
    }
}