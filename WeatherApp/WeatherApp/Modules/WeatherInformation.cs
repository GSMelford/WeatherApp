using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WeatherApp.Modules
{
    public class WeatherInformation : INotifyPropertyChanged
    {
        private string _location { get; set; }
        private string _lastUpdatedTime { get; set; }
        private string _temperature { get; set; }
        private string _temperatureFeelsLike { get; set; }
        private string _windSpeed { get; set; }
        private string _windDirect { get; set; }
        private string _pressure { get; set; }
        private string _weatherStatus { get; set; }

        public string Location
        {
            get => _location;
            set
            {
                if (_location == value) return;
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        public string LastUpdatedTime
        {
            get => _lastUpdatedTime;
            set
            {
                if (_lastUpdatedTime == value) return;
                _lastUpdatedTime = value;
                OnPropertyChanged(nameof(LastUpdatedTime));
            }
        }

        public string Temperature
        {
            get => _temperature;
            set
            {
                if (_temperature == value) return;
                _temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }

        public string TemperatureFeelsLike
        {
            get => _temperatureFeelsLike;
            set
            {
                if (_temperatureFeelsLike == value) return;
                _temperatureFeelsLike = value;
                OnPropertyChanged(nameof(TemperatureFeelsLike));
            }
        }

        public string WindSpeed
        {
            get => _windSpeed;
            set
            {
                if (_windSpeed == value) return;
                _windSpeed = value;
                OnPropertyChanged(nameof(WindSpeed));
            }
        }

        public string WindDirect
        {
            get => _windDirect;
            set
            {
                if (_windDirect == value) return;
                _windDirect = value;
                OnPropertyChanged(nameof(WindDirect));
            }
        }

        public string Pressure
        {
            get => _pressure;
            set
            {
                if (_pressure == value) return;
                _pressure = value;
                OnPropertyChanged(nameof(Pressure));
            }
        }

        public string WeatherStatus
        {
            get => _weatherStatus;
            set
            {
                if (_weatherStatus == value) return;
                _weatherStatus = value;
                OnPropertyChanged(nameof(WeatherStatus));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}