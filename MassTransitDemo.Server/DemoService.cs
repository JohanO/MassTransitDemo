// Copyright © Tetra Pak 2015 All rights reserved. No part of this
// program may be reproduced, stored in a retrieval system, or
// transmitted, in any form or by any means, without the prior
// permission, in writing, of Tetra Pak.

using System;

using MassTransit;
using MassTransit.Policies;

using Topshelf;

namespace MassTransitDemo.Server
{
    public class DemoService : ServiceControl
    {
        private readonly Uri BaseUri = new Uri("rabbitmq://localhost");

        private IBusControl _bus;

        public bool Start(HostControl hostControl)
        {
            _bus = ConfigureBus();
            _bus.Start();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _bus?.Stop();

            return true;
        }

        private IBusControl ConfigureBus() =>
            Bus.Factory.CreateUsingRabbitMq(config =>
            {
                var host = config.Host(BaseUri, hostConfig =>
                {
                    hostConfig.Username("guest");
                    hostConfig.Password("guest");
                });

                config.ReceiveEndpoint(host, "MassTransitDemo_CommandQueue", e =>
                {
                    e.UseRetry(new ImmediateRetryPolicy(new AllPolicyExceptionFilter(), 2));
                    e.Consumer<DoStuffConsumer>();
                    e.Consumer<DoOtherStuffConsumer>();
                    e.Consumer<DoBadStuffConsumer>();
                });
            });
    }
}