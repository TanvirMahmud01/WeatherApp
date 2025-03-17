using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;
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
                selectedCity = value;
                OnPropertyChanged("SelectedCity");
            }
        }

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
                        Value = -11
                    }
                }
            };
            }
        }

        public async Task MakeQuery()
        {
            var cities = await AccuWeatherHelper.GetCities(Query);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
