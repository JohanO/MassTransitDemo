﻿using System;

using MassTransit;
using MassTransit.Policies;

using MassTransitDemo.Contract;
using MassTransitDemo.TrafficLightServer.Domain;
using MassTransitDemo.TrafficLightServer.Handlers;
using MassTransitDemo.TrafficLightServer.Persistance;

using Microsoft.Practices.Unity;

using Topshelf;

namespace MassTransitDemo.TrafficLightServer
{
    public class Service : ServiceControl
    {
        private readonly Uri BaseUri = new Uri("rabbitmq://localhost");
        private readonly IUnityContainer _container = new UnityContainer();

        private IBusControl _bus;
        
        public Service()
        {
            _container.RegisterType<ISaga, Saga>();
            _container.RegisterType<IRepository, Repository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IConsumer<ICreateTrafficLightCommand>, CreateTrafficLightCommandConsumer>();
            _container.RegisterType<IConsumer<IGoCommand>, GoCommandConsumer>();
            _container.RegisterType<IConsumer<IStopCommand>, StopCommandConsumer>();
            _container.RegisterType<IConsumer<IStateQuery>, StateQueryConsumer>();
        }

        public bool Start(HostControl hostControl)
        {
            _bus = ConfigureBus();
            _container.RegisterInstance(_bus);

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

                config.ReceiveEndpoint(host, "MassTransitDemo_TrafficLightQueue", e =>
                    {
                        e.UseRetry(new ImmediateRetryPolicy(new AllPolicyExceptionFilter(), 2));
                        e.LoadFrom(_container);
                    });
            });
    }
}