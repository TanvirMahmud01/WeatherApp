using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;
using WeatherApp.ViewModel.Commands;
using WeatherApp.ViewModel.Helpers;

namespace WeatherApp.ViewModel
{
    public class WeatherVM : INotifyPropertyChanged
    {
        private string query;

        public string Query
        {
            get { return query; }
            set { 
                query = value;
                OnPropertyChanged("Query");
                }
        }

        public ObservableCollection<City> Cities { get; set; }

        private CurrentConditions currentConditions;

        public CurrentConditions CurrentConditions
        {
            get { return currentConditions; }
            set 
            { 
                currentConditions = value;
                OnPropertyChanged("CurrentConditions");
            }
        }

        private City selectedCity;

        public City SelectedCity
        {
            get { return selectedCity; }
            set {
                if (value == null)
                {
                    Console.WriteLine("SelectedCity is being set to null.");
                    return;
                }
                selectedCity = value;
                OnPropertyChanged("SelectedCity");
                if (selectedCity != null)
                {
                    GetCurrentConditions();
                }


            }
          
        }

        public SearchCommand SearchCommand { get; set; }

        public WeatherVM()
        {
            if(DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {

        
            SelectedCity = new City
            {
                LocalizedName = "Edmonton"
            };
            CurrentConditions = new CurrentConditions
            {
                WeatherText = "Partly Cloudy",
                Temperature = new Temperature
                {
                    Metric = new Units
                    {
                        Value = "-11"
                    }
                }
            };
            }

            SearchCommand = new SearchCommand(this);
            Cities = new ObservableCollection<City>();
        }

        private async void GetCurrentConditions()
        {

            if (selectedCity == null)
            {
                // Handle the null case
                Console.WriteLine("SelectedCity is null.");
                return;
            }

            try
            {
                Query = string.Empty;
                Cities.Clear();
                var conditions = await AccuWeatherHelper.GetCurrentConditions(selectedCity.Key);

                if (conditions == null)
                {
                    // Handle the case where conditions are null
                    Console.WriteLine("CurrentConditions is null.");
                    return;
                }

                CurrentConditions = conditions;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Exception in GetCurrentConditions: {ex.Message}");
            }
        }

        public async Task MakeQuery()
        {
            var cities = await AccuWeatherHelper.GetCities(Query);

            Cities.Clear();
            foreach( var city in cities)
            {
                Cities.Add(city);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
