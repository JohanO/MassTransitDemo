using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

using MassTransit;

using MassTransitDemo.Contract;

namespace MassTransitDemo.TrafficLightDisplay.ViewModel
{
    public class MainViewModel
    {
        public static MainViewModel Current { get; private set; }

        public MainViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                TrafficLights.Add(new TrafficLightViewModel(1) {RedOn = true});
                TrafficLights.Add(new TrafficLightViewModel(2) {YellowOn = true});
                TrafficLights.Add(new TrafficLightViewModel(3) {GreenOn = true});
            }

            Current = this;
            QueryState();
        }

        public ObservableCollection<TrafficLightViewModel> TrafficLights { get; } = new ObservableCollection<TrafficLightViewModel>();

        private async void QueryState()
        {
            var client = App.Bus.CreatePublishRequestClient<IStateQuery, IStateQueryResult>(TimeSpan.FromSeconds(10));
            var result = await client.Request(new StateQuery());
            var viewModels = result.States.Select(x => new TrafficLightViewModel(x));
            await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    foreach (var viewModel in viewModels)
                    {
                        TrafficLights.Add(viewModel);
                    }
                }).Task;
        }

        private class StateQuery : IStateQuery { }
    }
}