using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiBud.Models;
using MiBud.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.LiveParameter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LiveParameterSelectedPage : ContentPage
    {

        LiveParameterSelectedViewModel viewModel;
        public LiveParameterSelectedPage(ObservableCollection<PidCode> pidCodes)
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new LiveParameterSelectedViewModel(this, pidCodes);
                //viewModel.EMSParameterList = SelectedParameterList;
                //ems_List.ItemsSource = viewModel.EMSParameterList;
            }
            catch (Exception ex)
            {
            }
        }
    }
}