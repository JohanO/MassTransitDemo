using System.Threading.Tasks;

using MassTransit;

using MassTransitDemo.Contract;
using MassTransitDemo.TrafficLightServer.Persistance;

using Stateless;

namespace MassTransitDemo.TrafficLightServer.Domain
{
    public class Saga : ISaga
    {
        private readonly IBusControl _bus;
        private readonly IRepository _repository;
        private readonly StateMachine<State, Trigger> _stateMachine;

        private int _trafficLightId;

        public Saga(IBusControl bus, IRepository repository)
        {
            _bus = bus;
            _repository = repository;

            _stateMachine = new StateMachine<State, Trigger>(
                () => _repository.FindById(_trafficLightId).State,
                state => _repository.Update(_trafficLightId, state));
            
            _stateMachine.Configure(State.Initial)
                .Permit(Trigger.ToRed, State.Red);

            _stateMachine.Configure(State.Red)
                .OnEntryAsync(() => SendLightChangedAsync("Red", true))
                .Permit(Trigger.ToRedYellow, State.RedYellow);

            _stateMachine.Configure(State.RedYellow)
                .OnEntryAsync(EnterRedYellow)
                .OnExitAsync(ExitRedYellow)
                .Permit(Trigger.ToGreen, State.Green);

            _stateMachine.Configure(State.Green)
                .OnEntryAsync(() => SendLightChangedAsync("Green", true))
                .OnExitAsync(() => SendLightChangedAsync("Green", false))
                .Permit(Trigger.ToYellow, State.Yellow);

            _stateMachine.Configure(State.Yellow)
                .OnEntryAsync(EnterYellow)
                .OnExitAsync(() => SendLightChangedAsync("Yellow", false))
                .Permit(Trigger.ToRed, State.Red);
        }

        public bool CanFire(int trafficLIghtId, Trigger trigger)
        {
            _trafficLightId = trafficLIghtId;
            return _stateMachine.CanFire(trigger);
        }

        public Task FireAsync(int trafficLightId, Trigger trigger)
        {
            _trafficLightId = trafficLightId;
            return _stateMachine.FireAsync(trigger);
        }

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
                TrafficLightId = _trafficLightId,
                Color = color,
                IsOn = isOn
            });

        private async void Delay(Trigger trigger, int milliseconds)
        {
            await Task.Delay(milliseconds);
            await _stateMachine.FireAsync(trigger);
        }
    }
}