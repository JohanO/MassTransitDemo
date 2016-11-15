using System;
using System.Threading.Tasks;

using MassTransit;
using MassTransit.Policies;

using MassTransitDemo.Contract;

namespace MassTransitDemo.Client
{
    public static class Program
    {
        private static readonly Uri BaseUri = new Uri("rabbitmq://localhost");
        private static readonly Random Random = new Random();

        public static void Main(string[] args)
        {
            Task.Run(RunAsync).Wait();
        }

        private static async Task<int> RunAsync()
        {
            var bus = StartBus();
            var stuffClient = bus.CreatePublishRequestClient<IDoStuff, ICommandValidationResult>(TimeSpan.FromSeconds(10));
            var otherClient = bus.CreatePublishRequestClient<IDoOtherStuff, ICommandValidationResult>(TimeSpan.FromSeconds(10));

            Console.WriteLine("1. Send DoStuff");
            Console.WriteLine("2. Send DoOtherStuff");
            Console.WriteLine("3. Send DoBadStuff");

            while (true)
            {
                var choice = Console.ReadKey();
                Console.WriteLine();

                ICommandValidationResult result = null;
                switch (choice.KeyChar)
                {
                    case '1':
                        result = await stuffClient.Request(new DoStuff(Random.Next(10), 2.3));
                        Console.WriteLine($"Result: {result?.Message}");
                        break;

                    case '2':
                        result = await otherClient.Request(new DoOtherStuff(Random.Next(20)));
                        Console.WriteLine($"Result: {result?.Message}");
                        break;

                    case '3':
                        var endpoint = await bus.GetSendEndpoint(new Uri(BaseUri, @"\MassTransitDemo_CommandQueue"));
                        await endpoint.Send<IDoBadStuff>(new { });
                        await Task.Delay(1000);
                        break;
                }
            }
        }

        private static IBusControl StartBus()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(config =>
            {
                var host = config.Host(BaseUri, hostConfig =>
                {
                    hostConfig.Username("guest");
                    hostConfig.Password("guest");
                });
                config.ReceiveEndpoint(host, "MassTransitDemo_Subscriber", e => e.Consumer<EventHandler>());
            });

            bus.Start();
            return bus;
        }
    }
}
