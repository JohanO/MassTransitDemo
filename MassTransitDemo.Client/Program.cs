﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;

using MassTransit;
using MassTransit.Policies;

using MassTransitDemo.Contract;

using static System.Console;

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

        private static async Task RunAsync()
        {
            var bus = await StartBus();
            var stuffClient = bus.CreatePublishRequestClient<IDoStuff, ICommandValidationResult>(TimeSpan.FromSeconds(10));
            var otherClient = bus.CreatePublishRequestClient<IDoOtherStuff, ICommandValidationResult>(TimeSpan.FromSeconds(10));

            WriteLine("1. Send DoStuff");
            WriteLine("2. Send DoOtherStuff");
            WriteLine("3. Send DoBadStuff");
            WriteLine("4. Send CreateTrafficLight");
            WriteLine("5. Send Go");
            WriteLine("6. Send Stop");
            WriteLine("0. Exit");

            var exit = false;
            while (!exit)
            {
                var choice = ReadKey();
                WriteLine();
                switch (choice.KeyChar)
                {
                    case '1':
                        await stuffClient.Request(new DoStuff(Random.Next(10), 2.3)).WriteResultAsync();
                        break;

                    case '2':
                        var number = Random.Next(4);
                        WriteLine($"Sending DoOtherStuff, Number = {number}");
                        await otherClient.Request(new DoOtherStuff(number)).WriteResultAsync();
                        break;

                    case '3':
                        var endpoint = await bus.GetSendEndpoint(new Uri(BaseUri, @"\MassTransitDemo_CommandQueue"));
                        await endpoint.Send<IDoBadStuff>(new { });
                        await Task.Delay(1000);
                        break;

                    case '4':
                        await CreateTrafficLight(bus).WriteResultAsync();
                        break;

                    case '5':
                        await SendGo(bus).WriteResultAsync();
                        break;

                    case '6':
                        await SendStop(bus).WriteResultAsync();
                        break;

                    case '0':
                        exit = true;
                        break;

                    default:
                        WriteLine($"Invalid choice {choice.KeyChar}");
                        break;
                }
            }

            await bus.StopAsync();
        }

        private static Task<ICommandValidationResult> CreateTrafficLight(IBusControl bus)
        {
            var client = bus.CreatePublishRequestClient<ICreateTrafficLightCommand, ICommandValidationResult>(TimeSpan.FromSeconds(10));
            return client.Request(new CreateTrafficLightCommand { TrafficLightId = ReadTrafficLightId() });
        }

        private static Task<ICommandValidationResult> SendGo(IBusControl bus)
        {
            var client = bus.CreatePublishRequestClient<IGoCommand, ICommandValidationResult>(TimeSpan.FromSeconds(10));
            return client.Request(new GoCommand { TrafficLightId = ReadTrafficLightId() });
        }

        private static Task<ICommandValidationResult> SendStop(IBusControl bus)
        {
            var client = bus.CreatePublishRequestClient<IStopCommand, ICommandValidationResult>(TimeSpan.FromSeconds(10));
            return client.Request(new StopCommand { TrafficLightId = ReadTrafficLightId() });
        }

        private static int ReadTrafficLightId()
        {
            Write("Enter traffic light id: ");
            var id = int.Parse(ReadKey().KeyChar.ToString());
            WriteLine();
            return id;
        }

        private static async Task<IBusControl> StartBus()
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

            await bus.StartAsync();
            return bus;
        }
    }
}
