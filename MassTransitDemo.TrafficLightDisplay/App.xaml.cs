using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using MassTransit;
using MassTransit.Policies;

namespace MassTransitDemo.TrafficLightDisplay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Uri BaseUri = new Uri("rabbitmq://localhost");

        public static IBusControl Bus { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Bus = ConfigureBus();
            Bus.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Bus.Stop();
        }

        private IBusControl ConfigureBus() =>
            MassTransit.Bus.Factory.CreateUsingRabbitMq(config =>
            {
                var host = config.Host(BaseUri, hostConfig =>
                {
                    hostConfig.Username("guest");
                    hostConfig.Password("guest");
                });

                config.ReceiveEndpoint(host, "MassTransitDemo_Display", e =>
                {
                    e.Consumer<Model.EventHandler>();
                    e.AutoDelete = true;
                    e.Durable = false;
                });
            });
    }
}
