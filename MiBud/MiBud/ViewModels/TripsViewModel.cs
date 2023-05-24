using MiBud.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MiBud.ViewModels
{
    public class TripsViewModel : BaseViewModel
    {
        public TripsViewModel()
        {
            trip_list = new ObservableCollection<TripsModel>
            {
                new TripsModel
                {
                    trip_id= "trip_1",
                    kms = "123",
                    from_date = "22-08-2020",
                    to_date = "23-08-2020",
                },
                new TripsModel
                {
                    trip_id= "trip_2",
                    kms = "156",
                    from_date = "11-08-2020",
                    to_date = "13-08-2020",
                },
                new TripsModel
                {
                    trip_id= "trip_3",
                    kms = "300",
                    from_date = "01-08-2020",
                    to_date = "03-08-2020",
                },
            };
        }

        private ObservableCollection<TripsModel> _trip_list;
        public ObservableCollection<TripsModel> trip_list
        {
            get => _trip_list;
            set
            {
                _trip_list = value;
                OnPropertyChanged("trip_list ");
            }
        }
    }
}
