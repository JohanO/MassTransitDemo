using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace MassTransitDemo.TrafficLightDisplay.ViewModel
{
    public class TrafficLightViewModel : INotifyPropertyChanged
    {
        private bool _redOn;
        private bool _yellowOn;
        private bool _greenOn;

        public TrafficLightViewModel(int id)
        {
            Id = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; }

        public bool RedOn
        {
            get
            {
                return _redOn;
            }

            set
            {
                _redOn = value;
                OnPropertyChanged();
            }
        }

        public bool YellowOn
        {
            get
            {
                return _yellowOn;
            }
            set
            {
                _yellowOn = value;
                OnPropertyChanged();
            }
        }

        public bool GreenOn
        {
            get
            {
                return _greenOn;
            }
            set
            {
                _greenOn = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}