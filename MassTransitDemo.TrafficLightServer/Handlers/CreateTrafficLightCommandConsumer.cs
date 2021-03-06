﻿using System;
using System.Threading.Tasks;

using MassTransit;

using MassTransitDemo.Contract;
using MassTransitDemo.TrafficLightServer.Domain;
using MassTransitDemo.TrafficLightServer.Persistance;

namespace MassTransitDemo.TrafficLightServer.Handlers
{
    public class CreateTrafficLightCommandConsumer : IConsumer<ICreateTrafficLightCommand>
    {
        private readonly ISaga _saga;
        private readonly IRepository _repository;

        public CreateTrafficLightCommandConsumer(ISaga saga, IRepository repository)
        {
            _saga = saga;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ICreateTrafficLightCommand> context)
        {
            var cmd = context.Message;
            await Console.Out.WriteLineAsync($"Received ICreateTrafficLightCommand: Id={cmd.TrafficLightId};");

            if (_repository.Exists(cmd.TrafficLightId))
            {
                await context.RespondResultAsync(true, "Already exist");
            }
            else
            {
                await context.RespondResultAsync(false, "Validated OK");

                // Create the light
                _repository.Insert(new TrafficLight { Id = cmd.TrafficLightId });
                await context.Publish<ITrafficLightCreatedEvent>(new { cmd.TrafficLightId });

                // Turn on the red light
                await _saga.FireAsync(cmd.TrafficLightId, Trigger.ToRed);
            }
        }
    }
}