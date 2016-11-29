using System.Linq;
using System.Threading.Tasks;

using MassTransit;

using MassTransitDemo.Contract;
using MassTransitDemo.TrafficLightDisplay.ViewModel;

namespace MassTransitDemo.TrafficLightDisplay.Model
{
    public class EventHandler : 
        IConsumer<ITrafficLightCreatedEvent>,
        IConsumer<ILightChangedEvent>
    {
        public Task Consume(ConsumeContext<ITrafficLightCreatedEvent> context) =>
            App.Current.Dispatcher.InvokeAsync(() =>
                MainViewModel.Current.TrafficLights.Add(new TrafficLightViewModel(context.Message.TrafficLightId))).Task;

        public Task Consume(ConsumeContext<ILightChangedEvent> context) =>
            App.Current.Dispatcher.InvokeAsync(() =>
            {
                var trafficLight = MainViewModel.Current.TrafficLights.First(x => x.Id == context.Message.TrafficLightId);
                switch (context.Message.Color)
                {
                    case "Red":
                        trafficLight.RedOn = context.Message.IsOn;
                        break;

                    case "Yellow":
                        trafficLight.YellowOn = context.Message.IsOn;
                        break;

                    case "Green":
                        trafficLight.GreenOn = context.Message.IsOn;
                        break;
                }
            }).Task;
    }
}