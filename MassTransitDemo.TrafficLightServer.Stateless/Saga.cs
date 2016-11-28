using System;
using System.Net.Mail;
using System.Threading.Tasks;

using MassTransit;

using MassTransitDemo.Contract;

using Stateless;

namespace MassTransitDemo.TrafficLightServer.Stateless
{
    public class Saga
    {
        private readonly IBusControl _bus;
        private readonly TrafficLight _trafficLight;

        public Saga(IBusControl bus, TrafficLight trafficLight)
        {
            _bus = bus;
            _trafficLight = trafficLight;

            StateMachine = new StateMachine<State, Trigger>(
                () => _trafficLight.State,
                state => _trafficLight.State = state);

            StateMachine.Configure(State.Initial)
                .Permit(Trigger.ToRed, State.Red);

            StateMachine.Configure(State.Red)
                .OnEntryAsync(() => SendLightChangedAsync("Red", true))
                .Permit(Trigger.ToRedYellow, State.RedYellow);

            StateMachine.Configure(State.RedYellow)
                .OnEntryAsync(EnterRedYellow)
                .OnExitAsync(ExitRedYellow)
                .Permit(Trigger.ToGreen, State.Green);

            StateMachine.Configure(State.Green)
                .OnEntryAsync(() => SendLightChangedAsync("Green", true))
                .OnExitAsync(() => SendLightChangedAsync("Green", false))
                .Permit(Trigger.ToYellow, State.Yellow);

            StateMachine.Configure(State.Yellow)
                .OnEntryAsync(EnterYellow)
                .OnExitAsync(() => SendLightChangedAsync("Yellow", false))
                .Permit(Trigger.ToRed, State.Red);
        }

        public StateMachine<State, Trigger> StateMachine { get; private set; }

        private Task EnterRedYellow()
        {
            Delay(Trigger.ToGreen, 2000);
            return SendLightChangedAsync("Yellow", true);
        }

        private Task ExitRedYellow() => 
            Task.WhenAll(
                SendLightChangedAsync("Red", false),
                SendLightChangedAsync("Yellow", false));

        private Task EnterYellow()
        {
            Delay(Trigger.ToRed, 2000);
            return SendLightChangedAsync("Yellow", true);
        }

        private Task SendLightChangedAsync(string color, bool isOn) => 
            _bus.Publish<ILightChangedEvent>(new
            {
                TrafficLightId = _trafficLight.Id,
                Color = color,
                IsOn = isOn
            });

        private async void Delay(Trigger trigger, int milliseconds)
        {
            await Task.Delay(milliseconds);
            await StateMachine.FireAsync(trigger);
        }
    }
}