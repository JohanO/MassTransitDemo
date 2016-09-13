using System;
using System.Threading.Tasks;

using MassTransit;

using MassTransitDemo.Contract;

namespace MassTransitDemo.Server
{
    public class DoStuffConsumer : IConsumer<IDoStuff>
    {
        public async Task Consume(ConsumeContext<IDoStuff> context)
        {
            var cmd = context.Message;
            await Console.Out.WriteLineAsync($"Received IDoStuff command: Id={cmd.CommandId}; Number={cmd.StuffNumber}; Size={cmd.StuffSize}");

            await context.RespondAsync<ICommandValidationResult>(new
            {
                cmd.CommandId,
                cmd.CorrelationId,
                Fail = false,
                Message = "Validated OK"
            });

            await Console.Out.WriteLineAsync("Processing Stuff...");
            await Task.Delay(3000);
            await Console.Out.WriteLineAsync("Done Stuff!");

            await context.Publish<IStuffDone>(new
            {
                cmd.StuffNumber,
                cmd.StuffSize,
                Timestamp = 123.4
            });
        }
    }
}

