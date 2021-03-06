﻿using System;
using System.Threading.Tasks;

using MassTransit;

using MassTransitDemo.Contract;
using MassTransitDemo.TrafficLightServer.Domain;
using MassTransitDemo.TrafficLightServer.Persistance;

namespace MassTransitDemo.TrafficLightServer.Handlers
{
    public class GoCommandConsumer : IConsumer<IGoCommand>
    {
        private readonly ISaga _saga;
        private readonly IRepository _repository;

        public GoCommandConsumer(ISaga saga, IRepository repository)
        {
            _saga = saga;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IGoCommand> context)
        {
            var cmd = context.Message;
            await Console.Out.WriteLineAsync($"Received IGoCommand: Id={cmd.TrafficLightId};");

            if (!_repository.Exists(cmd.TrafficLightId))
            {
                await context.RespondResultAsync(true, $"TrafficLight {cmd.TrafficLightId} does not exist");
            }
            else if (!_saga.CanFire(cmd.TrafficLightId, Trigger.ToRedYellow))
            {
                await context.RespondResultAsync(true, $"TrafficLight {cmd.TrafficLightId} is in wrong state");
            }
            else
            {
                await context.RespondResultAsync(false, "Validated OK");
                await _saga.FireAsync(cmd.TrafficLightId, Trigger.ToRedYellow);
            }
        }
    }
}