using System;

using Automatonymous;

using MassTransit;
using MassTransit.Policies;

using Topshelf;

namespace MassTransitDemo.TrafficLightServer.Automatonymous
{
    public class TrafficLightService : ServiceControl
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

                config.ReceiveEndpoint(host, "MassTransitDemo_CommandQueue_Automatonymus", e =>
                {
                    e.UseRetry(new ImmediateRetryPolicy(new AllPolicyExceptionFilter(), 2));
                    //e.StateMachineSaga();
                    //e.Consumer<DoStuffConsumer>();
                    //e.Consumer<DoOtherStuffConsumer>();
                    //e.Consumer<DoBadStuffConsumer>();
                });
            });
    }
}