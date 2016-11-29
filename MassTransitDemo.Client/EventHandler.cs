// Copyright © Tetra Pak 2015 All rights reserved. No part of this
// program may be reproduced, stored in a retrieval system, or
// transmitted, in any form or by any means, without the prior
// permission, in writing, of Tetra Pak.

using System.Linq;
using System.Threading.Tasks;

using MassTransit;
using MassTransitDemo.Contract;

using static System.Console;

namespace MassTransitDemo.Client
{
    public class EventHandler :
        IConsumer<IStuffDone>,
        IConsumer<IOtherStuffDone>,
        IConsumer<Fault>,
        IConsumer<ITrafficLightCreatedEvent>,
        IConsumer<ILightChangedEvent>
    {
        public Task Consume(ConsumeContext<IStuffDone> context) =>
            Out.WriteLineAsync($"StuffDone: StuffNumber = {context.Message.StuffNumber}");

        public Task Consume(ConsumeContext<IOtherStuffDone> context) =>
            Out.WriteLineAsync($"OtherStuffDone: {context.Message.OtherStuffNumber}");

        public Task Consume(ConsumeContext<Fault> context) =>
            Out.WriteLineAsync($"Fault: {context.Message.Exceptions[0].Message}");

        public Task Consume(ConsumeContext<ITrafficLightCreatedEvent> context) =>
            Out.WriteLineAsync($"TrafficLightCreated: Id={context.Message.TrafficLightId}");

        public Task Consume(ConsumeContext<ILightChangedEvent> context) =>
            Out.WriteLineAsync($"TrafficLight Id={context.Message.TrafficLightId} Color={context.Message.Color} IsOn={context.Message.IsOn}");
    }
}