using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

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
        }

        public ObservableCollection<TrafficLightViewModel> TrafficLights { get; } = new ObservableCollection<TrafficLightViewModel>();
    }
}