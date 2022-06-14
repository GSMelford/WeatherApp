using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeatherApp.Modules;
using WeatherApp.WeatherModules;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp;

public partial class MainPage
{
    private bool _isPageLoad;
    private int _weatherProviderIndex;
    private int _degreePickerIndex;
    private int _daysPickerIndex;
    private const string LOCATION_KEY = "city";
    private readonly WeatherInformation _weatherInformation = new (); 
    private static List<Guid> _afterConstructorIds = new ();
    private ForecastData _lastForecastData;


    private static WeatherUpdater _weatherUpdater;
        
    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        Binding();

        _weatherUpdater = new WeatherUpdater(_weatherInformation);
            
        FirstWeatherProviderPicker.SelectedIndex = SecondWeatherProviderPicker.SelectedIndex = _weatherProviderIndex = 0;
        FirstDegreeProviderPicker.SelectedIndex = SecondDegreeProviderPicker.SelectedIndex = _degreePickerIndex = 0;
        DaysPicker.SelectedIndex = _daysPickerIndex = 4;
        
        base.OnAppearing();
        _isPageLoad = true;
        await UpdateWeather();
        await UpdateForecast();
    }

    private void Binding()
    {
        LocationLabel.SetBinding(Label.TextProperty, new Binding { Source = _weatherInformation, Path = "Location" });
        TemperatureLabel.SetBinding(Label.TextProperty, new Binding { Source = _weatherInformation, Path = "Temperature" });
        FeelsLikeTemperatureLabel.SetBinding(Label.TextProperty, new Binding { Source = _weatherInformation, Path = "TemperatureFeelsLike" });
        WeatherStatusLabel.SetBinding(Label.TextProperty, new Binding { Source = _weatherInformation, Path = "WeatherStatus" });
            
        LastUpdatedTimeLabel.SetBinding(Label.TextProperty, new Binding { Source = _weatherInformation, Path = "LastUpdatedTime" });
        WindSpeedLabel.SetBinding(Label.TextProperty, new Binding { Source = _weatherInformation, Path = "WindSpeed" });
        WindDirectLabel.SetBinding(Label.TextProperty, new Binding { Source = _weatherInformation, Path = "WindDirect" });
        PressureLabel.SetBinding(Label.TextProperty, new Binding { Source = _weatherInformation, Path = "Pressure" });
    }
    
    private async void ChangeCity(object sender, EventArgs e)
    {
        if (sender is not Entry entry) return;
        Preferences.Set(LOCATION_KEY, entry.Text);
        await UpdateWeather();
        await UpdateForecast();
    }

    private void DegreeChanged(object sender, EventArgs e)
    {
        if (sender is not Picker picker) return;
        _degreePickerIndex = picker.SelectedIndex;
        _weatherUpdater.UpdateWeatherInfo(_degreePickerIndex);

        if (_lastForecastData is null)
        {
            return;
        }
        
        ForecastStackLayout.Children.Clear();
        foreach (var element in BuildForecast(_lastForecastData, _degreePickerIndex))
        {
            var tempElement = (Frame) element;
            ForecastStackLayout.Children.Add(tempElement);
        }
    }

    private async void OnWhetherProviderPickerChanged(object sender, EventArgs e)
    {
        if (sender is not Picker picker) return;
        _weatherProviderIndex = picker.SelectedIndex;
        await UpdateWeather();
        await UpdateForecast();
    }

    private async void OnDaysPickerChanged(object sender, EventArgs e)
    {
        if (sender is not Picker picker) return;
        _daysPickerIndex = picker.SelectedIndex;
        await UpdateWeather();
        await UpdateForecast();
    }
    
    private async Task UpdateWeather()
    {
        if (!_isPageLoad)
        {
            return;
        }
        
        await _weatherUpdater.SetCurrentWeather(_weatherProviderIndex, Preferences.Get(LOCATION_KEY, "Kiev"));
        _weatherUpdater.UpdateWeatherInfo(_degreePickerIndex);
        
        if (!string.IsNullOrEmpty(_weatherUpdater.GetWeatherIcon()))
        {
            WeatherIconImage.Source = ImageSource.FromStream(
                () =>  new MemoryStream(new WebClient().DownloadData(_weatherUpdater.GetWeatherIcon())));
        }
    }

    private async Task UpdateForecast()
    {
        if (!_isPageLoad)
        {
            return;
        }

        _lastForecastData = await _weatherUpdater.GetForecast(
            _weatherProviderIndex,
            Preferences.Get(LOCATION_KEY, "Kiev"),
            _daysPickerIndex + 1);
        
        ForecastStackLayout.Children.Clear();
        foreach (var element in BuildForecast(_lastForecastData, _degreePickerIndex))
        {
            var tempElement = (Frame) element;
            ForecastStackLayout.Children.Add(tempElement);
        }
    }
   
    private IList<Element> BuildForecast(ForecastData forecastData, int degree)
    {
        _afterConstructorIds = new List<Guid>();
        _lastForecastData = forecastData;
        
        List<Element> elements = new List<Element>();
        foreach (var weatherDayData in forecastData.WeatherDayDataList)
        {
            Frame frame = new Frame
            {
                BackgroundColor = Color.FromHex("#282828"),
                Margin = new Thickness(4),
                Padding = new Thickness(2),
                CornerRadius = 4
            };

            Grid grid = new Grid
            {
                RowDefinitions = { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }},
                ColumnDefinitions = 
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }
                }
            };

            Button button = new Button
            {
                Text = "Info",
                TextColor = Color.Snow,
                BackgroundColor = Color.Black
            };
            
            button.Clicked += ButtonOnClicked;
            grid.Children.Add(new StackLayout {Margin = 2, Children =
            {
                button
            }}, 0, 0);
            
            grid.Children.Add(new StackLayout {Children =
            {
                new Image
                {
                    Source = ImageSource.FromStream(
                        () =>  new MemoryStream(new WebClient().DownloadData(weatherDayData.WeatherIconUrl))),
                    Aspect = Aspect.Fill,
                    WidthRequest = 50,
                    HeightRequest = 50
                }
            }}, 1, 0);
            
            grid.Children.Add(new StackLayout {Margin = 2, Children =
            {
                new Label {Text = $"Max: {(degree == 0 ? weatherDayData.TemperatureMaxC : weatherDayData.TemperatureMaxF)}°", TextColor = Color.Snow},
                new Label {Text = $"Min: {(degree == 0 ? weatherDayData.TemperatureMinC : weatherDayData.TemperatureMinF)}°", TextColor = Color.Snow}
            }}, 2, 0);
            
            grid.Children.Add(new StackLayout {Margin = 2, Children =
            {
                new Label {Text = $"Average: {(degree == 0 ? weatherDayData.AverageTemperatureC : weatherDayData.AverageTemperatureF)}", TextColor = Color.Snow}
            }}, 3, 0);
            
            grid.Children.Add(new StackLayout {Margin = 2, Children =
            {
                new Label {Text = weatherDayData.WeatherStatus, TextColor = Color.Snow},
                new Label {Text = weatherDayData.Date, TextColor = Color.Snow}
            }}, 4, 0);
            
            frame.Content = grid;
            elements.Add(frame);
            
            _afterConstructorIds.Add(button.Id);
        }

        return elements;
    }

    private async void ButtonOnClicked(object sender, EventArgs e)
    {
        if (sender is not Button button) return;
        int index = _afterConstructorIds.FindIndex(guid => guid == button.Id);
        
        WeatherDayData weatherDayData = _lastForecastData.WeatherDayDataList[index];
        await DisplayActionSheet($"Weather fo day {weatherDayData.Date}", "Cancel", "Ok", weatherDayData.WeatherDataHours
            .Select(
                x => $"{x.Date} {x.TemperatureC}° {x.WeatherStatus} WS: {x.WindSpeed}")
            .ToArray());
    }
}