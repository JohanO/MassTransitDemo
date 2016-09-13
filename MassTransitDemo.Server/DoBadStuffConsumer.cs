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
    public class DoBadStuffConsumer : IConsumer<IDoBadStuff>
    {
        public async Task Consume(ConsumeContext<IDoBadStuff> context)
        {
            await Console.Out.WriteLineAsync("Doing something bad!");
            throw new Exception("Something bad happened!");
        }
    }
}