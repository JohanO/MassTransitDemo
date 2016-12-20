// Copyright © Tetra Pak 2015 All rights reserved. No part of this
// program may be reproduced, stored in a retrieval system, or
// transmitted, in any form or by any means, without the prior
// permission, in writing, of Tetra Pak.

using System;
using System.Threading.Tasks;

using MassTransit;

using MassTransitDemo.Contract;

namespace MassTransitDemo.Server
{
    public class DoOtherStuffConsumer : IConsumer<IDoOtherStuff>
    {
        public async Task Consume(ConsumeContext<IDoOtherStuff> context)
        {
            var cmd = context.Message;
            await Console.Out.WriteLineAsync($"Received IDoOtherStuff command: Number={cmd.OtherStuffNumber}");

            await context.RespondAsync<ICommandValidationResult>(new
            {
                cmd.CommandId,
                cmd.CorrelationId,
                Fail = false,
                Message = "Other validation passed"
            });

            await Console.Out.WriteLineAsync("Processing other stuff...");
            await Task.Delay(5000);
            await Console.Out.WriteLineAsync($"Done other stuff, Number={cmd.OtherStuffNumber}");

            await context.Publish<IOtherStuffDone>(new { cmd.OtherStuffNumber });
        }
    }
}