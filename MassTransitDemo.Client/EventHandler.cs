// Copyright © Tetra Pak 2015 All rights reserved. No part of this
// program may be reproduced, stored in a retrieval system, or
// transmitted, in any form or by any means, without the prior
// permission, in writing, of Tetra Pak.

using System.Threading.Tasks;

using MassTransit;
using MassTransitDemo.Contract;

using static System.Console;

namespace MassTransitDemo.Client
{
    public class EventHandler :
        IConsumer<IStuffDone>,
        IConsumer<IOtherStuffDone>
    {
        public async Task Consume(ConsumeContext<IStuffDone> context) => 
            await Out.WriteLineAsync($"StuffDone: StuffNumber = {context.Message.StuffNumber}");

        public async Task Consume(ConsumeContext<IOtherStuffDone> context)
        {
            await Out.WriteLineAsync($"OtherStuffDone: {context.Message.OtherStuffNumber}");
        }
    }
}