﻿using MiBud.Models;
using MiBud.ViewModels;
using MiBud.Views.CreateServiceTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.VehicleService
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehicleServicePage : ContentPage
    {
        VehicleServiceViewModel viewModel;
        public VehicleServicePage(Vehicle selected_vehicle)
        {
            InitializeComponent();
            BindingContext = viewModel = new VehicleServiceViewModel(this, selected_vehicle);
        }

        //private void add_new_vehicle_service_Clicked(object sender, EventArgs e)
        //{
        //    Navigation.PushAsync(new CreateServiceTicketPage());
        //}

        //private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        //{

        //}

        //private void action_clicked(object sender, EventArgs e)
        //{

        //}

        //private void ImageButton_Clicked(object sender, EventArgs e)
        //{

        //}
    }
}